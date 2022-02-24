using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Handlers.Abstractions;
using Fluegram.Handlers.Middlewares;
using TypeExtensions = Fluegram.Filters.TypeExtensions;

namespace Fluegram.Handlers;

public static class PipelineBuilderExtensions
{
    public static IPipelineBuilder<TEntityContext, TEntity> UseHandlers<TEntityContext, TEntity>(
        this IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder,
        Action<HandlersConfigurator<TEntityContext, TEntity>> configureHandlers)
        where TEntityContext : IEntityContext<TEntity>
        where TEntity : class
    {
        HandlersConfigurator<TEntityContext, TEntity> handlersConfigurator = new(pipelineBuilder);

        configureHandlers(handlersConfigurator);

        return pipelineBuilder;
    }

    public class HandlersConfigurator<TEntityContext, TEntity>
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        private readonly IPipelineBuilder<TEntityContext, TEntity> _pipelineBuilder;

        internal HandlersConfigurator(IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder)
        {
            _pipelineBuilder = pipelineBuilder;
        }

        public HandlersConfigurator<TEntityContext, TEntity> Use<THandler>()
            where THandler : class, IHandler<TEntityContext, TEntity>
        {
            _pipelineBuilder.Components.RegisterType<THandler>().IfNotRegistered(typeof(THandler));

            var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, THandler>().ToList();

            var preProcessingActions = Actions.TypeExtensions
                .FindPreProcessingActionsFor<TEntityContext, TEntity, THandler>().ToList();
            var postProcessingActions = Actions.TypeExtensions
                .FindPostProcessingActionsFor<TEntityContext, TEntity, THandler>().ToList();

            _pipelineBuilder.Components.RegisterType<HandlerMiddleware<THandler, TEntityContext, TEntity>>()
                .WithParameters(new[]
                {
                    new PositionalParameter(0, filters),
                    new PositionalParameter(1, preProcessingActions),
                    new PositionalParameter(2, postProcessingActions)
                });

            _pipelineBuilder.Use<HandlerMiddleware<THandler, TEntityContext, TEntity>>();

            return this;
        }
    }
}