namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandArgumentsSuccessfulParseResult<TArguments> : ICommandArgumentsParseResult<TArguments>
    where TArguments : class, new()
{
    TArguments Arguments { get; }

    string Text { get; }
}