namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandArgumentParseError
{
    ICommandArgument Argument { get; }
    
    Exception? Exception { get; }
}