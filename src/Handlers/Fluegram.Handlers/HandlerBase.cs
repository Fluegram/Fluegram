using Fluegram.Handlers.Abstractions;
using Fluegram.Types.Contexts;

namespace Fluegram.Handlers;

public abstract class HandlerBase<TEntity> : IHandler<EntityContext<TEntity>, TEntity> where TEntity : class
{
    public abstract Task HandleAsync(EntityContext<TEntity> entityContext, CancellationToken cancellationToken);
}