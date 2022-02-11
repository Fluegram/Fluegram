using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Fluegram.Commands.Abstractions;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Filters.Abstractions.Filtering;

namespace Fluegram.Commands.Middlewares;

public class
    DefaultCommandMiddleware<TCommand, TEntityContext, TEntity> : CommandMiddlewareBase<TEntityContext, TEntity>
    where TCommand : ICommand<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    public override async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        if (!await InvokeFiltersAsync(context, cancellationToken).ConfigureAwait(false))
            return;

        TCommand command = context.Components.Resolve<TCommand>();

        ICommandNameRetriever commandNameRetriever = context.Components.Resolve<ICommandNameRetriever>();

        string commandName = commandNameRetriever.Retrieve(context, command.Id);

        IEntityTextManipulator<TEntity> entityTextManipulator =
            context.Components.Resolve<IEntityTextManipulator<TEntity>>();

        string text = entityTextManipulator.Get(context.Entity);

        if (IsMatches(text, commandName, out var resultText))
        {
            entityTextManipulator.Set(context.Entity, resultText);

            await InvokePreProcessingActionsAsync(context, cancellationToken).ConfigureAwait(false);

            await command.ProcessAsync(context, cancellationToken).ConfigureAwait(false);

            await InvokePostProcessingActionsAsync(context, cancellationToken).ConfigureAwait(false);

            context.Cancel();
        }
    }

    public DefaultCommandMiddleware(IEnumerable<IFilter<TEntityContext, TEntity>> filters,
        IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions,
        IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions) : base(filters,
        preProcessingActions, postProcessingActions)
    {
    }
}

public class
    DefaultCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity> : CommandMiddlewareBase<TEntityContext,
        TEntity>
    where TCommand : ICommand<TEntityContext, TEntity, TArguments>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
    where TArguments : class, new()
{
    

    public override async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        if (!await InvokeFiltersAsync(context, cancellationToken).ConfigureAwait(false))
            return;

        TCommand command = context.Components.Resolve<TCommand>();

        ICommandNameRetriever commandNameRetriever = context.Components.Resolve<ICommandNameRetriever>();

        string commandName = commandNameRetriever.Retrieve(context, command.Id);

        IEntityTextManipulator<TEntity> entityTextManipulator =
            context.Components.Resolve<IEntityTextManipulator<TEntity>>();

        string text = entityTextManipulator.Get(context.Entity);

        if (IsMatches(text, commandName, out var resultText))
        {
            entityTextManipulator.Set(context.Entity, resultText);

            ICommandArgumentsParser commandArgumentsParser = context.Components.Resolve<ICommandArgumentsParser>();

            string argumentsText = entityTextManipulator.Get(context.Entity);

            ICommandArgumentsParseResult<TArguments> parseResult =
                commandArgumentsParser.Parse<TArguments>(argumentsText);

            if (parseResult is ICommandArgumentsSuccessfulParseResult<TArguments> { Arguments: { } arguments })
            {
                await command.ProcessAsync(context, arguments, cancellationToken).ConfigureAwait(false);

                context.Cancel();
            }
            else if (parseResult is ICommandArgumentsFailedParseResult<TArguments> { Errors: { } errors })
            {
                await command.ProcessInvalidArgumentsAsync(context, errors, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public DefaultCommandMiddleware(IEnumerable<IFilter<TEntityContext, TEntity>> filters, IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions, IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions) : base(filters, preProcessingActions, postProcessingActions)
    {
    }
}