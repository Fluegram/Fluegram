namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandArgumentsFailedParseResult<TArguments>: ICommandArgumentsParseResult<TArguments>
    where TArguments : class, new()
{
    IEnumerable<ICommandArgumentParseError> Errors { get; }
}