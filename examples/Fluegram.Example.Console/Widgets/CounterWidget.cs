using Fluegram.Example.Console.Widgets.States;
using Fluegram.Parameters;
using Fluegram.Types.Contexts;
using Fluegram.Widgets;
using Telegram.Bot.Types;

namespace Fluegram.Example.Console.Widgets;

public class CounterWidget : WidgetBase<Message, CounterState>
{
    private Message _sentMessage;
    
    public override async Task OnInitializedAsync(EntityContext<Message> entityContext, CancellationToken cancellationToken)
    {
        _sentMessage =
            await entityContext.InvokeAsync<SendTextMessageParameters, Message>(_ => _.UseText("Initiaizing counter widget"), cancellationToken: cancellationToken);
    }

    public override async Task OnUpdatedAsync(EntityContext<Message> entityContext, CounterState state, CancellationToken cancellationToken)
    {
        await entityContext.InvokeAsync<EditMessageTextParameters, Message>(_ => _
            .UseMessageId(_sentMessage.MessageId)
            .UseText($"Counter: {state.Counter}"), cancellationToken: cancellationToken);
    }
}
