using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Widgets.Abstractions;

public interface IWidgetController<TEntityContext, TEntity, TState> : IAsyncDisposable
    where TState : class, IWidgetState<TState>, new()
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    TState State { get; }
    Task NotifyStateChangedAsync(CancellationToken cancellationToken = default);
}