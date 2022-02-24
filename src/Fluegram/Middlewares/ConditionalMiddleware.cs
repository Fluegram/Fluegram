using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Middlewares;

public class ConditionalMiddleware<TEntityContext, TEntity> : IConditionalMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public ConditionalMiddleware(IPipeline<TEntityContext, TEntity> innerPipeline,
        MiddlewareConditionDelegate<TEntityContext, TEntity> condition)
    {
        InnerPipeline = innerPipeline;
        Condition = condition;
    }

    public async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        if (await Condition(context, cancellationToken).ConfigureAwait(false))
            await InnerPipeline.ProcessEntityAsync(context, cancellationToken).ConfigureAwait(false);
    }

    public MiddlewareConditionDelegate<TEntityContext, TEntity> Condition { get; }
    public IPipeline<TEntityContext, TEntity> InnerPipeline { get; }
}