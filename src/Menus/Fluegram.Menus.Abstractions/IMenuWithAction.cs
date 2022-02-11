using Fluegram.Abstractions.Types.Contexts;
using Telegram.Bot.Types;

namespace Fluegram.Menus.Abstractions;

public interface IMenuWithAction<TEntityContext> : IMenu
    where TEntityContext : IEntityContext<CallbackQuery>
{
    Task InvokeAsync(TEntityContext entityContext, CancellationToken cancellationToken);
}