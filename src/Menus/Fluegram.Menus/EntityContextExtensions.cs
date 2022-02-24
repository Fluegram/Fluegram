using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Keyboards.Builders.Inline;
using Fluegram.Menus.Abstractions;
using Fluegram.Parameters;
using Telegram.Bot.Types;

namespace Fluegram.Menus;

public static class EntityContextExtensions
{
    public static Task SendMenuAsync<TMenu>(this IContext context, CancellationToken cancellationToken)
        where TMenu : IMenu
    {
        var menu = context.Components.Resolve<TMenu>();

        var builder = new InlineKeyboardMarkupBuilder();

        var parents = new List<string>();

        var current = menu.Parent;

        while (current != null)
        {
            parents.Add(current.Id);

            current = current.Parent;
        }

        parents.Reverse();

        foreach (var item in menu)
            builder.UseRow(_ =>
                _.UseCallbackData(item.Name(context), $"{string.Join(":", parents)}:{menu.Id}:{item.Id}"));

        if (menu.Parent is { } parent)
            builder.UseRow(_ =>
                _.UseCallbackData($"Return back to {parent.Name(context)}", $"{string.Join(":", parents)}"));

        var markup = builder.Build();

        return context.InvokeAsync<SendTextMessageParameters, Message>(_ => _
            .UseText(menu.Text(context))
            .UseReplyMarkup(markup), cancellationToken);
    }
}