using Fluegram.Menus;
using Fluegram.Menus.Abstractions;
using Fluegram.Parameters;
using Fluegram.Sessions;
using Fluegram.Types.Contexts;
using Telegram.Bot.Types;

namespace Fluegram.Example.Console.Menus;

public class EditProfileNameMenu : MenuBase, IMenuWithAction<EntityContext<CallbackQuery>>
{
    public EditProfileNameMenu() : base("name", _ => "Edit Profile Name", _ => "Name")
    {
    }

    public async Task InvokeAsync(EntityContext<CallbackQuery> entityContext, CancellationToken cancellationToken)
    {
        var session = entityContext.Session<EntityContext<Message>, Message>();

        var context = new EntityContext<Message>(entityContext.Entity.Message, entityContext);

        var firstName = await RequestNameAsync("first");
        var lastName = await RequestNameAsync("last");

        await entityContext.InvokeAsync<SendTextMessageParameters, Message>(_ =>
            _.UseText($"Your profile information ({firstName} {lastName}) has been saved ."), cancellationToken);

        async Task<string> RequestNameAsync(string kind)
        {
            var message = await session!.RequestAsync(context!, _ => _
                .UseAction((state, ctx, token) =>
                {
                    return ctx.InvokeAsync<SendTextMessageParameters, Message>(_ =>
                        _.UseText($"Please, specify your {kind} name:"));
                })
                .UseMatcher(async (state, ctx, token) => state.Entity!.Text is not null), cancellationToken)!;

            return message!.Text ?? message.Caption!;
        }
    }
}