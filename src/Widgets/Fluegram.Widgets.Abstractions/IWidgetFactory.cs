using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Widgets.Abstractions;

public interface IWidgetFactory<TWidget, TEntityContext, TEntity, TState> : IAsyncDisposable
    where TWidget : IWidget<TEntityContext, TEntity, TState>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
    where TState :  class, IWidgetState<TState>, new()
{
    Task<IWidgetController<TEntityContext, TEntity, TState>> CreateAsync(TEntityContext entityContext, CancellationToken cancellationToken);
}