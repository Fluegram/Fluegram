namespace Fluegram.SourceGenerator.Parameters;

public static class FluentExtensions
{
    public static T If<T>(this T t, bool condition, Action<T> action)
    {
        if (condition) action(t);

        return t;
    }
}