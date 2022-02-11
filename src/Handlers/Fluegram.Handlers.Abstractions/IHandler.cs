using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Handlers.Abstractions;

public interface IHandler<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    Task HandleAsync(TEntityContext entityContext, CancellationToken cancellationToken);
}