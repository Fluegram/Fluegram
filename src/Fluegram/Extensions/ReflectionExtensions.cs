using System.Reflection;

namespace Fluegram.Extensions;

internal static class ReflectionExtensions
{
    public static MethodInfo? GetMethod(this object source, string name,
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic)
    {
        return source.GetType().GetMethod(name, bindingFlags);
    }
}