using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public delegate bool TryParseDelegate<T>(string source, out T? result);

public class FuncCommandArgumentTypeResolver<T> : ICommandArgumentTypeResolver<T>
{
    private readonly Func<string, T?> _resolverFunc;

    public FuncCommandArgumentTypeResolver(Func<string, T?> resolverFunc)
    {
        _resolverFunc = resolverFunc;
    }

    public FuncCommandArgumentTypeResolver(TryParseDelegate<T> parseDelegate) : this(source =>
        parseDelegate(source, out T? result) ? result : default)
    {
        
    }

    public T? Resolve(string source) => _resolverFunc(source);
}