namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandArgumentsParser<TArguments>
    where TArguments : class, new()
{
    ICommandArgumentsParseResult<TArguments> Parse(string sourceText);
}