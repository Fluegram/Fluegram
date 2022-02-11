namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandArgumentsParseResult<TArguments>
    where TArguments : class, new()
{
    bool Success { get; }
}