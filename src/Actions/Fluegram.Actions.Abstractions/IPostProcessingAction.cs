using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Actions.Abstractions;

public interface IPostProcessingAction<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    Task InvokeAsync(TEntityContext entityContext, CancellationToken cancellationToken);
}