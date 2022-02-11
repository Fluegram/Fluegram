using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public class CommandArgumentParseError : ICommandArgumentParseError
{
    public CommandArgumentParseError(ICommandArgument argument, Exception? exception)
    {
        Argument = argument;
        Exception = exception;
    }

    public ICommandArgument Argument { get; }
    public Exception? Exception { get; }
}