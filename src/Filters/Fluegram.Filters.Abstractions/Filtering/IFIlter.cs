using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Filters.Abstractions.Filtering;

public interface IFilter<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    Task<bool> MatchesAsync(TEntityContext entityContext, CancellationToken cancellationToken);
}