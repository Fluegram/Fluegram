using Fluegram.Abstractions.Middlewares;
using Fluegram.Types.Contexts;

namespace Fluegram.Middlewares;

public abstract class MiddlewareBase<TEntity> : IMiddleware<EntityContext<TEntity>, TEntity> where TEntity : class
{
    public abstract Task ProcessAsync(EntityContext<TEntity> context, CancellationToken cancellationToken = default);
}