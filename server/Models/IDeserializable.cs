
using server.Database;

namespace server.Models;

public interface IDeserializable
{
    public void Deserialize(MySqlCustomReader reader);
}