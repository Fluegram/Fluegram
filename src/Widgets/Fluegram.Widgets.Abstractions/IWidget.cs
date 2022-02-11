using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Widgets.Abstractions;

public interface IWidget<TEntityContext, TEntity, TState> : IAsyncDisposable
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
    where TState : class, IWidgetState<TState>, new()
{
    Task OnInitializedAsync(TEntityContext entityContext, CancellationToken cancellationToken);

    Task OnUpdatedAsync(TEntityContext entityContext, TState state, CancellationToken cancellationToken);
}