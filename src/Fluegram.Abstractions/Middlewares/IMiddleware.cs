using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Abstractions.Middlewares;

public interface IMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken);
}