using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Fluegram.Filters.Abstractions.Filtering;
using Fluegram.Handlers.Abstractions;
using Fluegram.Handlers.Abstractions.Middlewares;

namespace Fluegram.Handlers.Middlewares;

public class
    HandlerMiddleware<THandler, TEntityContext, TEntity> : IHandlerMiddleware<THandler, TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
    where THandler : IHandler<TEntityContext, TEntity>
{
    public HandlerMiddleware(
        IEnumerable<IFilter<TEntityContext, TEntity>> filters,
        IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions,
        IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions)
    {
        Filters = filters;
        PreProcessingActions = preProcessingActions;
        PostProcessingActions = postProcessingActions;
    }

    public async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        foreach (var filter in Filters)
            if (!await filter.MatchesAsync(context, cancellationToken).ConfigureAwait(false))
                return;

        var handler = context.Components.Resolve<THandler>();

        foreach (var preProcessingAction in PreProcessingActions)
            await preProcessingAction.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

        await handler.HandleAsync(context, cancellationToken).ConfigureAwait(false);

        foreach (var postProcessingAction in PreProcessingActions)
            await postProcessingAction.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
    }

    public IEnumerable<IFilter<TEntityContext, TEntity>> Filters { get; }
    public IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> PreProcessingActions { get; }
    public IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> PostProcessingActions { get; }
}