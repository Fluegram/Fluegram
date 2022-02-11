using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Abstractions.Middlewares;

public delegate Task<bool> MiddlewareConditionDelegate<TEntityContext, TEntity>(TEntityContext context, CancellationToken cancellationToken)
    where TEntityContext : IEntityContext<TEntity> where TEntity : class;

public interface IConditionalMiddleware<TEntityContext, TEntity> : IMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    MiddlewareConditionDelegate<TEntityContext, TEntity> Condition { get; }

    IPipeline<TEntityContext, TEntity> InnerPipeline { get; }
}