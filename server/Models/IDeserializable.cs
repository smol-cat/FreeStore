
using MySql.Data.MySqlClient;

namespace server.Models;

public interface IDeserializable
{
    public void Deserialize(MySqlDataReader reader);
}