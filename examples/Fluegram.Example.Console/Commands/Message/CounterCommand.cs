using Fluegram.Commands;
using Fluegram.Example.Console.Widgets;
using Fluegram.Example.Console.Widgets.States;
using Fluegram.Types.Contexts;
using Fluegram.Widgets.Abstractions;

namespace Fluegram.Example.Console.Commands.Message;

public class CounterCommand : CommandBase<Telegram.Bot.Types.Message>
{
    private readonly IWidgetFactory<CounterWidget, EntityContext<Telegram.Bot.Types.Message>, Telegram.Bot.Types.Message, CounterState> _widgetFactory;

    public CounterCommand(IWidgetFactory<CounterWidget, EntityContext<Telegram.Bot.Types.Message>, Telegram.Bot.Types.Message, CounterState> widgetFactory) : base("counter")
    {
        _widgetFactory = widgetFactory;
    }

    public override async Task ProcessAsync(EntityContext<Telegram.Bot.Types.Message> entityContext, CancellationToken cancellationToken)
    {
        IWidgetController<EntityContext<Telegram.Bot.Types.Message>,Telegram.Bot.Types.Message,CounterState> widgetController 
            = await _widgetFactory.CreateAsync(entityContext, cancellationToken);

        while (true)
        {
            await Task.Delay(3000, cancellationToken);

            widgetController.State.Counter++;

            await widgetController.NotifyStateChangedAsync(cancellationToken);
        }
    }
}