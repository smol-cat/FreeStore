using System.Text.RegularExpressions;

namespace tests.utils;

public static class Regexes
{
    public static Regex CreatedId = new(@"\d+$");
}