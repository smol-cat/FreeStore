
using server.Database;

namespace server.Utilities;

public class TokenCache
{
    private static DbConnection conn;
    private static HashSet<string> _cache;

    public TokenCache(DbConnection connection)
    {
        if (_cache == null)
        {
            conn = connection;
            if (!connection.SelectAndDeserialize("SELECT token FROM blocked_token WHERE NOW() < expiration_date", new(), out var tokens))
            {
                throw (new Exception("Could not cache blocked tokens"));
            }

            _cache = new HashSet<string>(tokens.Select(e => (string)e["token"]).ToList());

            Console.WriteLine("Tokens are loaded");
        }
    }

    public bool BlockToken(string token, DateTime expiration)
    {
        if (!conn.Execute("INSERT INTO blocked_token (token, expiration_date) VALUES (@token, @expiration)", new() { ["token"] = token, ["expiration"] = expiration }))
        {
            return false;
        }

        _cache.Add(token);
        return true;
    }

    public bool IsBlocked(string token) => _cache.Contains(token);
}