using Fluegram.Commands.Attributes;

namespace Fluegram.Example.Console.Commands.Message;

public class RandomArguments
{
    [CommandArgument] public int Min { get; set; }

    [CommandArgument] public int Max { get; set; }
}