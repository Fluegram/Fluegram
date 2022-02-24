using System.Security.Cryptography;
using System.Text;
using Fluegram.Commands;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Parameters;
using Fluegram.Types.Contexts;
using Telegram.Bot;

namespace Fluegram.Example.Console.Commands.Message;

public class RandomCommand : CommandBase<Telegram.Bot.Types.Message, RandomArguments>
{
    public RandomCommand() : base("random")
    {
    }

    public override async Task ProcessAsync(EntityContext<Telegram.Bot.Types.Message> entityContext,
        RandomArguments arguments, CancellationToken cancellationToken)
    {
        var randomValue = RandomNumberGenerator.GetInt32(arguments.Min, arguments.Max);

        await entityContext.Client.SendTextMessageAsync(entityContext.Chat!,
            $"Random value from {arguments.Min} to {arguments.Max} is {randomValue}.",
            cancellationToken: cancellationToken);
    }

    public override async Task ProcessInvalidArgumentsAsync(EntityContext<Telegram.Bot.Types.Message> entityContext,
        IEnumerable<ICommandArgumentParseError> argumentsParseErrors,
        CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();

        sb.AppendLine("One of arguments you've entered is invalid. Or one of arguments was missing:");

        var index = 1;
        foreach (var error in argumentsParseErrors)
            if (error.Exception is { Message: { } message })
                sb.AppendLine($"{index++}. {error.Argument.Name} - {message}");
            else
                sb.AppendLine($"{index++}. {error.Argument.Name}.");

        await entityContext.InvokeAsync<SendTextMessageParameters, Telegram.Bot.Types.Message>(_ =>
            _.UseText(sb.ToString()), cancellationToken);
    }
}