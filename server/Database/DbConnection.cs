

using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using server.Models;

namespace server.Database;

public class DbConnection : DbContext
{
    private static MySqlConnection conn;
    private static string _connectionString;
    public Exception LastException { get; private set; }

    private Dictionary<Type, MySqlDbType> TypeMap = new()
    {
        [typeof(string)] = MySqlDbType.VarChar,
        [typeof(int)] = MySqlDbType.Int32,
        [typeof(bool)] = MySqlDbType.Int16,
        [typeof(DateTime?)] = MySqlDbType.DateTime,
        [typeof(DateTime)] = MySqlDbType.DateTime,
        [typeof(byte[])] = MySqlDbType.LongBlob,
        [typeof(decimal)] = MySqlDbType.Decimal,
        [typeof(double)] = MySqlDbType.Double
    };

    public DbConnection(IConfiguration configuration)
    {
        if (conn == null)
        {
            _connectionString = $"server={configuration["DB:host"]};port=3306;database={configuration["DB:database"]};username={configuration["DB:username"]};password={configuration["DB:password"]};";
            conn = new MySqlConnection(_connectionString);
            conn.Open();
            Console.WriteLine($"MySQL version : {conn?.ServerVersion}");
        }
    }

    public long LastInsertedId { get; private set; }

    private MySqlCustomReader ExecuteQuery(MySqlCommand cmd)
    {
        cmd.Connection = conn;
        MySqlCustomReader reader = new MySqlCustomReader(cmd.ExecuteReader());
        LastInsertedId = cmd.LastInsertedId;
        return reader;
    }

    private bool HandleException(Exception e)
    {
        if (Regexes.RestartDatabase.IsMatch(e.Message))
        {
            conn = new MySqlConnection(_connectionString);
            conn.Open();
            return true;
        }
        LastException = e;
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
        return false;
    }

    private void ExecuteLogic(string query, Dictionary<string, object> parameters)
    {
        using var cmd = new MySqlCommand();
        parameters.ToList().ForEach(e => cmd.Parameters.Add($"@{e.Key}", TypeMap[e.Value.GetType()]).Value = e.Value);
        cmd.Connection = conn;
        cmd.CommandText = query;
        cmd.ExecuteNonQuery();
        LastInsertedId = cmd.LastInsertedId;
    }

    public bool Execute(string query, Dictionary<string, object> parameters)
    {
        try
        {
            ExecuteLogic(query, parameters);
        }
        catch (Exception e)
        {
            if (!HandleException(e))
            {
                return false;
            }

            ExecuteLogic(query, parameters);
        }

        return true;
    }

    private List<Dictionary<string, object>> SelectAndDeserializeLogic(string query, Dictionary<string, object> parameters, MySqlCustomReader customReader)
    {
        List<Dictionary<string, object>> result = new();
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText = query;
        parameters.ToList().ForEach(e => cmd.Parameters.Add($"@{e.Key}", TypeMap[e.Value.GetType()]).Value = e.Value);
        customReader = ExecuteQuery(cmd);
        var reader = customReader.Reader;
        while (reader.Read())
        {
            var entry = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                entry[reader.GetName(i)] = reader.GetValue(i);
            }
            result.Add(entry);
        }
        reader.Close();
        return result;
    }

    public bool SelectAndDeserialize(string query, Dictionary<string, object> parameters, out List<Dictionary<string, object>> result)
    {
        result = new();
        MySqlCustomReader customReader = null;

        try
        {
            result = SelectAndDeserializeLogic(query, parameters, customReader);
        }
        catch (Exception e)
        {
            if (!HandleException(e))
            {
                return false;
            }

            customReader?.Reader.Close();
            result = SelectAndDeserializeLogic(query, parameters, customReader);
        }

        return true;
    }

    private List<T> SelectAndDeserializeLogic<T>(string query, Dictionary<string, object> parameters, MySqlCustomReader customReader) where T : IDeserializable, new()
    {
        List<T> result = new();
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText = query;
        parameters.ToList().ForEach(e => cmd.Parameters.Add($"@{e.Key}", TypeMap[e.Value.GetType()]).Value = e.Value);
        customReader = ExecuteQuery(cmd);
        while (customReader.Reader.Read())
        {
            T entry = new();
            entry.Deserialize(customReader);
            result.Add(entry);
        }
        customReader.Reader.Close();
        return result;
    }

    public bool SelectAndDeserialize<T>(string query, Dictionary<string, object> parameters, out List<T> result) where T : IDeserializable, new()
    {
        MySqlCustomReader customReader = null;
        result = new();
        try
        {
            result = SelectAndDeserializeLogic<T>(query, parameters, customReader);
        }
        catch
        {
            if (!HandleException(e))
            {
                return false;
            }

            customReader?.Reader.Close();
            result = SelectAndDeserializeLogic<T>(query, parameters, customReader);
        }
        return true;
    }
}