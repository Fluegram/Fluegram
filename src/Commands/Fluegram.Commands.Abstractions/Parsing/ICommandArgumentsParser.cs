namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandArgumentsParser
{
    ICommandArgumentsParseResult<TArguments> Parse<TArguments>(string sourceText) where TArguments : class, new();
}