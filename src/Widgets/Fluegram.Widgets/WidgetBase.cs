using Fluegram.Types.Contexts;
using Fluegram.Widgets.Abstractions;

namespace Fluegram.Widgets;

public abstract class WidgetBase<TEntity, TState> : IWidget<EntityContext<TEntity>, TEntity, TState>
    where TEntity : class where TState : class, IWidgetState<TState>, new()
{
    public virtual ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public abstract Task OnInitializedAsync(EntityContext<TEntity> entityContext, CancellationToken cancellationToken);

    public abstract Task OnUpdatedAsync(EntityContext<TEntity> entityContext, TState state,
        CancellationToken cancellationToken);
}