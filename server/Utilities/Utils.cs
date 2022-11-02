

using System.Security.Cryptography;
using System.Text;

public static class Utils
{
    public static string SHA256String(string value)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    public static DateTime UnixTimeStampToDateTime(long timeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
        return dateTime;
    }
}