using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Widgets.Abstractions;

namespace Fluegram.Widgets;

public static class FluegramBotBuilderExtensions
{
    public static IPipelineFeaturesConfigurator<TEntityContext, TEntity> UseWidgets<TEntityContext, TEntity>(
        this IPipelineFeaturesConfigurator<TEntityContext, TEntity> featuresConfigurator, Action<WidgetsConfigurator<TEntityContext, TEntity>> configureWidgets)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        WidgetsConfigurator<TEntityContext, TEntity> configurator = new(featuresConfigurator.Components);

        configureWidgets(configurator);
        
        return featuresConfigurator;
    }

    public class WidgetsConfigurator<TEntityContext, TEntity> where TEntityContext : IEntityContext<TEntity>
        where TEntity : class
    {
        private readonly ContainerBuilder _containerBuilder;

        internal WidgetsConfigurator(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public WidgetsConfigurator<TEntityContext, TEntity> Use<TWidget, TState>()
            where TWidget : IWidget<TEntityContext, TEntity, TState> where TState : class, IWidgetState<TState>, new()
        {
            _containerBuilder.RegisterType<TWidget>();
            
            _containerBuilder
                .RegisterType<WidgetFactory<TWidget, TEntityContext, TEntity, TState>>()
                .AsImplementedInterfaces()
                .SingleInstance();

            return this;
        }
    }
}