using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public class CommandArgumentsFailedParseResult<TArguments> : CommandArgumentsParseResultBase<TArguments>,
    ICommandArgumentsFailedParseResult<TArguments>
    where TArguments : class, new()
{
    public CommandArgumentsFailedParseResult(bool success, IEnumerable<ICommandArgumentParseError> errors) : base(success)
    {
        Errors = errors;
    }

    public IEnumerable<ICommandArgumentParseError> Errors { get; }
}