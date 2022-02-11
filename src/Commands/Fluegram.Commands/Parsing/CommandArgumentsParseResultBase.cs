using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public abstract class CommandArgumentsParseResultBase<TArguments> : ICommandArgumentsParseResult<TArguments> where TArguments : class, new()
{
    protected CommandArgumentsParseResultBase(bool success)
    {
        Success = success;
    }

    public bool Success { get; }
}