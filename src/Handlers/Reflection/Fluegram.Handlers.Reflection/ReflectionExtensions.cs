using System.Reflection;

namespace Fluegram.Handlers.Reflection;

public static class ReflectionExtensions
{
    public static bool IsInstantiableType(this Type type)
    {
        return type is { IsAbstract: false, IsSealed: false };
    }

    public static bool IsTaskMethod(this MethodInfo methodInfo)
    {
        return methodInfo.ReturnType == typeof(Task);
    }
}