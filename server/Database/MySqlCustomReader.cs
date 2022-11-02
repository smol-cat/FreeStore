
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
        get
        {
            try
            {
                return Reader[key] is DBNull ? null : Reader[key];
            }
            catch (Exception _)
            {
                return null;
            }
        }
    }
}