using Fluegram.Commands;
using Fluegram.Menus;
using Fluegram.Menus.Abstractions;
using Fluegram.Types.Contexts;

namespace Fluegram.Example.Console.Commands.Message.Menus;

public abstract class SendMenuCommandBase<TMenu> : CommandBase<Telegram.Bot.Types.Message>
    where TMenu : IMenu
{
    protected SendMenuCommandBase(string id) : base(id)
    {
    }

    public override Task ProcessAsync(EntityContext<Telegram.Bot.Types.Message> entityContext, CancellationToken cancellationToken)
    {
        return entityContext.SendMenuAsync<TMenu>(cancellationToken);
    }
}