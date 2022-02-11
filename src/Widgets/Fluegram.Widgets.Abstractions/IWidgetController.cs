using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Widgets.Abstractions;


public delegate Task AsyncHandler<T>(T value, CancellationToken cancellationToken);
public interface IWidgetController<TEntityContext, TEntity, TState> : IAsyncDisposable
    where TState :  class, IWidgetState<TState>, new()
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    Task NotifyStateChangedAsync(CancellationToken cancellationToken = default);

    TState State { get; }
}