

using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using System.Reflection;

namespace server.Entities;

class DbConnection
{
    public static string Server = "localhost";
    public static int Port = 3306;
    public static string DataBaseName = "free_store";
    public static string UserName = "smol_cat";
    public static string Password = "2448";

    MySqlConnection conn = new MySqlConnection($"server={Server};port={Port};database={DataBaseName};username={UserName};password={Password};");
    private static DbConnection _instance = new DbConnection();

    public Exception LastException { get; private set; }

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

    private MySqlDataReader ExecuteQuery(string query)
    {
        using var cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = query;
        cmd.ExecuteNonQuery();
        LastInsertedId = cmd.LastInsertedId;
        return cmd.ExecuteReader();
    }

    public bool Execute(string query)
    {
        try
        {
            using var cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
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

    public bool SelectAndDeserialize(string query, out List<Dictionary<string, object>> result)
    {
        result = new();
        MySqlDataReader reader = null;
        try
        {
            reader = ExecuteQuery(query);
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
            reader?.Close();
            LastException = e;
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return false;
        }
    }

    public bool SelectAndDeserialize<T>(string query, out List<T> result) where T : new()
    {
        MySqlDataReader reader = null;
        result = new();
        try
        {
            reader = ExecuteQuery(query);
            while (reader.Read())
            {
                T entry = new();
                PropertyInfo[] properties = typeof(T).GetProperties() ?? throw new Exception($"Could not get properties of the type '{typeof(T).Name}'");
                Dictionary<string, PropertyInfo> mappedProperties = properties.ToList().ToDictionary(e => e.Name.ToLower(), e => e);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string dbProperty = reader.GetName(i).ToLower();
                    if (mappedProperties.TryGetValue(dbProperty, out PropertyInfo property))
                    {
                        try
                        {
                            property.SetValue(entry, reader.GetValue(i));
                        }
                        catch (ArgumentException e)
                        {
                            throw new ArgumentException($"Tried to convert field {reader.GetName(i)} to incompatible type: {e.Message}");
                        }
                    }
                }
                result.Add(entry);
            }
            reader.Close();
            return true;
        }
        catch (Exception e)
        {
            reader?.Close();
            LastException = e;
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return false;
        }
    }
}