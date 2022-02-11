using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public record struct CommandArgument(string Name) : ICommandArgument;