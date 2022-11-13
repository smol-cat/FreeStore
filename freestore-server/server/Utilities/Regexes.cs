

using System.Text.RegularExpressions;

public static class Regexes
{
    public static RegexOptions RegexOptionsCompiled = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant;
    public static Regex InvalidName = new(@"[\.\,\|\;\:<>\/""'\\\?\[\]\{\}\(\)\s!@#$%^&+\-*]", RegexOptionsCompiled);
    public static Regex Email = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptionsCompiled);
    public static Regex RestartDatabase = new(@"Connection must be valid and open|inactivity", RegexOptionsCompiled);
}