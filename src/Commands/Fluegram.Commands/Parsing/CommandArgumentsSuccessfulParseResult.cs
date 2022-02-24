using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public class CommandArgumentsSuccessfulParseResult<TArguments> : CommandArgumentsParseResultBase<TArguments>,
    ICommandArgumentsSuccessfulParseResult<TArguments>
    where TArguments : class, new()
{
    public CommandArgumentsSuccessfulParseResult(bool success, TArguments arguments, string text) : base(success)
    {
        Arguments = arguments;
        Text = text;
    }

    public TArguments Arguments { get; }
    public string Text { get; }
}