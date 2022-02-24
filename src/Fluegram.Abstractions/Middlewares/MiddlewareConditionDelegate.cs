using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Abstractions.Middlewares;

public delegate Task<bool> MiddlewareConditionDelegate<TEntityContext, TEntity>(TEntityContext context,
    CancellationToken cancellationToken)
    where TEntityContext : IEntityContext<TEntity> where TEntity : class;