using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Fluegram.Commands.Abstractions.Middlewares;
using Fluegram.Filters.Abstractions.Filtering;

namespace Fluegram.Commands.Middlewares;

public abstract class CommandMiddlewareBase<TEntityContext, TEntity> : ICommandMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public IEnumerable<IFilter<TEntityContext, TEntity>> Filters { get; }
    public IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> PreProcessingActions { get; }
    public IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> PostProcessingActions { get; }

    protected CommandMiddlewareBase(IEnumerable<IFilter<TEntityContext, TEntity>> filters,
        IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions,
        IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions)
    {
        Filters = filters;
        PreProcessingActions = preProcessingActions;
        PostProcessingActions = postProcessingActions;
    }

    public abstract Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken);

    protected async Task<bool> InvokeFiltersAsync(TEntityContext entityContext, CancellationToken cancellationToken)
    {
        foreach (var filter in Filters)
        {
            if (!await filter.MatchesAsync(entityContext, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }

    protected async Task InvokePreProcessingActionsAsync(TEntityContext entityContext,
        CancellationToken cancellationToken)
    {
        foreach (var action in PreProcessingActions)
        {
            await action.InvokeAsync(entityContext, cancellationToken).ConfigureAwait(false);
        }
    }
    
    protected async Task InvokePostProcessingActionsAsync(TEntityContext entityContext,
        CancellationToken cancellationToken)
    {
        foreach (var action in PreProcessingActions)
        {
            await action.InvokeAsync(entityContext, cancellationToken).ConfigureAwait(false);
        }
    }
    
    
    protected bool IsMatches(string text, string commandName, out string arguments)
    {
        arguments = text;
        
        const StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        string[] commandNameSegments = commandName.Split(' ', splitOptions);

        string[] textSegments = text.Split(' ', commandNameSegments.Length + 1, splitOptions);

        if (textSegments.Length < commandNameSegments.Length)
            return false;
        
        for (int i = 0; i < commandNameSegments.Length; i++)
        {
            string textSegment = textSegments[i];
            string commandNameSegment = commandNameSegments[i];

            if (string.CompareOrdinal(textSegment, commandNameSegment) != 0)
            {
                return false;
            }
        }

        arguments = string.Join(" ", textSegments.Except(commandNameSegments));
        
        return true;
    }
}