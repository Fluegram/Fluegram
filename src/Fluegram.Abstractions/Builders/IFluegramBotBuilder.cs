using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Abstractions.Builders;

public interface IFluegramBotBuilder : IBuilder<IFluegramBot, IComponentContext>
{
    ContainerBuilder Components { get; }
    
    IFluegramBotBuilder UseFor<TEntityContext, TEntity>(UpdateType updateType,
        Action<IPipelineFeaturesConfigurator<TEntityContext, TEntity>> configureFeatures,
        Action<IPipelineBuilder<TEntityContext, TEntity>> configurePipeline)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class;
    
    IFluegramBotBuilder UseFor<TEntityContext, TEntity>(UpdateType updateType,
        Action<IPipelineBuilder<TEntityContext, TEntity>> configurePipeline)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class;
}

public interface IPipelineFeaturesConfigurator<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    ContainerBuilder Components { get; }
}