using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Abstractions.Types.Descriptors;
using Fluegram.Middlewares;
using Fluegram.Pipelines;
using Fluegram.Types.Descriptors;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Builders;

public class PipelineBuilder<TEntityContext, TEntity> : IPipelineBuilder<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    private readonly UpdateType _updateType;
    private readonly IList<IMiddlewareDescriptor<TEntityContext, TEntity>> _middlewareDescriptors;

    public PipelineBuilder(UpdateType updateType, ContainerBuilder components)
    {
        _updateType = updateType;
        Components = components;
        _middlewareDescriptors = new List<IMiddlewareDescriptor<TEntityContext, TEntity>>();
    }


    public ContainerBuilder Components { get; }

    public IPipelineBuilder<TEntityContext, TEntity> Use<TMiddleware>() where TMiddleware : class, IMiddleware<TEntityContext, TEntity>
    {
        Components.RegisterType<TMiddleware>().IfNotRegistered(typeof(TMiddleware));
        
        _middlewareDescriptors.Add(MiddlewareDescriptor<TEntityContext, TEntity>.CreateFromMiddleware<TMiddleware>());

        return this;
    }

    public IPipelineBuilder<TEntityContext, TEntity> UseWhen(MiddlewareConditionDelegate<TEntityContext, TEntity> condition, Action<IPipelineBuilder<TEntityContext, TEntity>> configureInnerPipeline)
    {
        var innerPipelineBuilder = new PipelineBuilder<TEntityContext, TEntity>(_updateType, Components);

        configureInnerPipeline(innerPipelineBuilder);

        var innerPipeline = innerPipelineBuilder.Build();
        
        string name = Guid.NewGuid().ToString();

        Components.RegisterType<ConditionalMiddleware<TEntityContext, TEntity>>()
            .WithParameter(new PositionalParameter(0, innerPipeline))
            .WithParameter(new PositionalParameter(1, condition))
            .Named<ConditionalMiddleware<TEntityContext, TEntity>>(name);

        _middlewareDescriptors.Add(MiddlewareDescriptor<TEntityContext, TEntity>.CreateFromMiddleware<ConditionalMiddleware<TEntityContext, TEntity>>(name));
        
        return this;
    }

    public IPipelineBuilder<TEntityContext, TEntity> UseDescriptor<TMiddlewareDescriptor>(TMiddlewareDescriptor descriptor) where TMiddlewareDescriptor : IMiddlewareDescriptor<TEntityContext, TEntity>
    {
        _middlewareDescriptors.Add(descriptor);

        return this;
    }

    public IPipeline<TEntityContext, TEntity> Build()
    {
        return new Pipeline<TEntityContext, TEntity>(_updateType, _middlewareDescriptors);
    }
}