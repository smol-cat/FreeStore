
using MySql.Data.MySqlClient;

namespace server.Database;

public class MySqlCustomReader
{
    public MySqlDataReader Reader;
    public MySqlCustomReader(MySqlDataReader reader)
    {
        this.Reader = reader;
    }

    public object this[string key]
    {
        get => Reader[key] is DBNull ? null : Reader[key];
    }
}