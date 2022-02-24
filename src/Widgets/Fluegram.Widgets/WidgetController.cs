using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Widgets.Abstractions;

namespace Fluegram.Widgets;

public class WidgetController<TEntityContext, TEntity, TState> : IWidgetController<TEntityContext, TEntity, TState>
    where TState : class, IWidgetState<TState>, new()
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    private readonly TEntityContext _entityContext;
    private readonly IWidget<TEntityContext, TEntity, TState> _widget;

    public WidgetController(
        TEntityContext entityContext,
        IWidget<TEntityContext, TEntity, TState> widget)
    {
        _entityContext = entityContext;
        _widget = widget;
        State = new TState();
    }

    public async Task NotifyStateChangedAsync(CancellationToken cancellationToken = default)
    {
        await _widget.OnUpdatedAsync(_entityContext, State, cancellationToken).ConfigureAwait(false);
    }

    public TState State { get; private set; }

    public async ValueTask DisposeAsync()
    {
        State = default!;

        await _widget.DisposeAsync().ConfigureAwait(false);
    }
}