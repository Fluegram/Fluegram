using System.Collections.ObjectModel;
using Autofac;
using Fluegram.Abstractions;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Factories.Contexts;
using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Factories.Contexts;
using Fluegram.Types.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Builders;

public class FluegramBotBuilder : IFluegramBotBuilder
{
    private readonly IDictionary<UpdateType, IPipeline> _pipelines;

    public FluegramBotBuilder(
        ContainerBuilder components)
    {
        Components = components;
        _pipelines = new Dictionary<UpdateType, IPipeline>();
    }

    public IFluegramBot Build(IComponentContext componentContext)
    {
        return new FluegramBot(componentContext.Resolve<ITelegramBotClient>(), componentContext, new ReadOnlyDictionary<UpdateType, IPipeline>(_pipelines));
    }

    public ContainerBuilder Components { get; }
    
    public IFluegramBotBuilder UseFor<TEntityContext, TEntity>(UpdateType updateType, 
        Action<IPipelineFeaturesConfigurator<TEntityContext, TEntity>> configureFeatures,
        Action<IPipelineBuilder<TEntityContext, TEntity>> configurePipeline) where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        if (_pipelines.ContainsKey(updateType))
        {
            throw new InvalidOperationException("Cannot configure a new pipeline because there is already a pipeline with the specified update type.");
        }

        PipelineFeaturesConfigurator<TEntityContext, TEntity> configurator = new PipelineFeaturesConfigurator<TEntityContext, TEntity>(Components);

        configureFeatures(configurator);
        

        if (typeof(TEntityContext) == typeof(EntityContext<TEntity>))
        {
            Components.RegisterType<EntityContextFactory<TEntity>>().As<IEntityContextFactory<TEntityContext, TEntity>>();    
        }
        
        IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder = new PipelineBuilder<TEntityContext, TEntity>(updateType, Components);
        
        configurePipeline?.Invoke(pipelineBuilder);

        IPipeline<TEntityContext, TEntity> pipeline = pipelineBuilder.Build();
        
        _pipelines.Add(updateType, pipeline);

        return this;
    }

    public IFluegramBotBuilder UseFor<TEntityContext, TEntity>(UpdateType updateType, Action<IPipelineBuilder<TEntityContext, TEntity>> configurePipeline) where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        return UseFor(updateType, _ => { }, configurePipeline);
    }
}