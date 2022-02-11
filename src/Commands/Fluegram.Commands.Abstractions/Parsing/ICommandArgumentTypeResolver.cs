namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandArgumentTypeResolver<T>
{
    T? Resolve(string source);
}