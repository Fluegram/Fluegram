namespace Fluegram.SourceGenerator.Parameters;

public static class StringExtensions
{
    public static string ToPascal(this string source)
    {
        return new string(source.Select((ch, i) => i == 0 ? char.ToUpper(ch) : ch).ToArray());
    }

    public static string ToCamel(this string source)
    {
        return new string(source.Select((ch, i) => i == 0 ? char.ToLower(ch) : ch).ToArray());
    }
}