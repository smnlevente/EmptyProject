using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringExtension
{
    public static string ToUnderscoreCase(this string str)
    {
        var result = Regex.Replace(str, "(?<=[a-z0-9])[A-Z]", m => "_" + m.Value);
        return result.ToLowerInvariant();
    }

    public static string ToCamelCase(this string str)
    {
        return str
            .Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
            .Aggregate(string.Empty, (s1, s2) => s1 + s2);
    }
}
