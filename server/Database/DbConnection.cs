

using MySql.Data.MySqlClient;
using server.Models;

namespace server.Database;

public class DbConnection
{
    public static string Server = "localhost";
    public static int Port = 3306;
    public static string DataBaseName = "free_store";
    public static string UserName = "smol_cat";
    public static string Password = "2448";

    MySqlConnection conn = new MySqlConnection($"server={Server};port={Port};database={DataBaseName};username={UserName};password={Password};");
    private static DbConnection _instance = new DbConnection();

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

    private DbConnection()
    {
        conn.Open();
        Console.WriteLine($"MySQL version : {conn.ServerVersion}");
    }

    public static DbConnection Instance
    {
        get
        {
            return _instance;
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

    public bool Execute(string query, Dictionary<string, object> parameters)
    {
        try
        {
            using var cmd = new MySqlCommand();
            parameters.ToList().ForEach(e => cmd.Parameters.Add($"@{e.Key}", TypeMap[e.Value.GetType()]).Value = e.Value);
            cmd.Connection = conn;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            LastInsertedId = cmd.LastInsertedId;
            return true;
        }
        catch (Exception e)
        {
            LastException = e;
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return false;
        }
    }

    public bool SelectAndDeserialize(string query, Dictionary<string, object> parameters, out List<Dictionary<string, object>> result)
    {
        result = new();
        MySqlCustomReader customReader = null;
        try
        {
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
            return true;
        }
        catch (Exception e)
        {
            customReader?.Reader.Close();
            LastException = e;
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return false;
        }
    }

    public bool SelectAndDeserialize<T>(string query, Dictionary<string, object> parameters, out List<T> result) where T : IDeserializable, new()
    {
        MySqlCustomReader customReader = null;
        result = new();
        try
        {
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
            return true;
        }
        catch (Exception e)
        {
            customReader?.Reader.Close();
            LastException = e;
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return false;
        }
    }
}