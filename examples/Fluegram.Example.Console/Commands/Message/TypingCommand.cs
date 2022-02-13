using Fluegram.Commands;
using Fluegram.Types.Contexts;
using Fluegram.Actions.Attributes;
using Fluegram.Parameters;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Example.Console.Commands.Message;

[ChatActionAttribute<EntityContext<Telegram.Bot.Types.Message>, Telegram.Bot.Types.Message>(ChatAction.Typing)]
public class TypingCommand : CommandBase<Telegram.Bot.Types.Message>
{
    public TypingCommand() : base("probability")
    {
    }

    public override async Task ProcessAsync(EntityContext<Telegram.Bot.Types.Message> entityContext, CancellationToken cancellationToken)
    {
        await entityContext.InvokeAsync<SendTextMessageParameters, Telegram.Bot.Types.Message>(_ =>
            _.UseText("Message sent in 3 seconds after invocation of action."), cancellationToken: cancellationToken);
    }
}