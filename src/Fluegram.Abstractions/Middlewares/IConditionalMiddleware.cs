using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Abstractions.Middlewares;

public interface IConditionalMiddleware<TEntityContext, TEntity> : IMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    MiddlewareConditionDelegate<TEntityContext, TEntity> Condition { get; }

    IPipeline<TEntityContext, TEntity> InnerPipeline { get; }
}