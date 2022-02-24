using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Handlers.Abstractions;
using Fluegram.Keyboards.Builders.Inline;
using Fluegram.Menus.Abstractions;
using Fluegram.Parameters;
using Telegram.Bot.Types;

namespace Fluegram.Menus;

public class MenuHandler<TEntityContext, TMenu> : IHandler<TEntityContext, CallbackQuery>
    where TEntityContext : IEntityContext<CallbackQuery>
    where TMenu : IMenu
{
    public async Task HandleAsync(TEntityContext entityContext, CancellationToken cancellationToken)
    {
        var menu = entityContext.Components.Resolve<TMenu>();

        var callbackData = entityContext.Entity.Data!;

        var callbackDataSegments =
            callbackData.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        IEnumerable<IMenu> menuItems = menu;

        var parentSegments = new List<string> { menu.Id };

        if (string.CompareOrdinal(menu.Id, callbackDataSegments[0]) is 0)
        {
            var index = 1;

            IMenu selectedItem = menu;

            while (menuItems.Any() && index < callbackDataSegments.Length)
                foreach (var item in menuItems)
                    if (string.CompareOrdinal(item.Id, callbackDataSegments[index]) is 0)
                    {
                        selectedItem = item;
                        parentSegments.Add(item.Id);
                        menuItems = item;
                        index++;
                        break;
                    }

            if (selectedItem is { Text: { } text })
            {
                if (selectedItem.Any())
                {
                    var builder = new InlineKeyboardMarkupBuilder();

                    foreach (var item in selectedItem)
                        builder.UseRow(_ => _.UseCallbackData(item.Name(entityContext),
                            string.Join(":", parentSegments.Append(item.Id))));

                    if (parentSegments.Count > 1)
                        builder.UseRow(_ =>
                            _.UseCallbackData("Return Back", string.Join(":", parentSegments.SkipLast(1))));

                    var markup = builder.Build();

                    await entityContext.InvokeAsync<EditMessageTextParameters, Message>(_ => _
                        .UseMessageId(entityContext.Entity.Message!.MessageId)
                        .UseText(text(entityContext))
                        .UseReplyMarkup(markup), cancellationToken);
                    return;
                }

                if (selectedItem is IMenuWithAction<TEntityContext> menuWithAction)
                    await menuWithAction.InvokeAsync(entityContext, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}