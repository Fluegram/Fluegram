using Autofac;
using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Abstractions.Types.Descriptors;

namespace Fluegram.Abstractions.Builders;

public interface IPipelineBuilder<TEntityContext, TEntity> : IBuilder<IPipeline<TEntityContext, TEntity>>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    ContainerBuilder Components { get; }
    
    IPipelineBuilder<TEntityContext, TEntity> Use<TMiddleware>()
        where TMiddleware : class, IMiddleware<TEntityContext, TEntity>;

    IPipelineBuilder<TEntityContext, TEntity> UseWhen(MiddlewareConditionDelegate<TEntityContext, TEntity> condition,
        Action<IPipelineBuilder<TEntityContext, TEntity>> configureInnerPipeline);

    IPipelineBuilder<TEntityContext, TEntity> UseDescriptor<TMiddlewareDescriptor>(TMiddlewareDescriptor descriptor)
        where TMiddlewareDescriptor : IMiddlewareDescriptor<TEntityContext, TEntity>;
}