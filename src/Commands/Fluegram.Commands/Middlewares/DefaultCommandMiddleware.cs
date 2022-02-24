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
    public DefaultCommandMiddleware(IEnumerable<IFilter<TEntityContext, TEntity>> filters,
        IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions,
        IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions) : base(filters,
        preProcessingActions, postProcessingActions)
    {
    }

    public override async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        if (!await InvokeFiltersAsync(context, cancellationToken).ConfigureAwait(false))
            return;

        var command = context.Components.Resolve<TCommand>();

        var commandNameRetriever = context.Components.Resolve<ICommandNameRetriever>();

        var commandName = commandNameRetriever.Retrieve(context, command.Id);

        var entityTextManipulator =
            context.Components.Resolve<IEntityTextManipulator<TEntity>>();

        var text = entityTextManipulator.Get(context.Entity);

        if (IsMatches(text, commandName, out var resultText))
        {
            entityTextManipulator.Set(context.Entity, resultText);

            await InvokePreProcessingActionsAsync(context, cancellationToken).ConfigureAwait(false);

            await command.ProcessAsync(context, cancellationToken).ConfigureAwait(false);

            await InvokePostProcessingActionsAsync(context, cancellationToken).ConfigureAwait(false);

            context.Cancel();
        }
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
    public DefaultCommandMiddleware(IEnumerable<IFilter<TEntityContext, TEntity>> filters,
        IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions,
        IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions) : base(filters,
        preProcessingActions, postProcessingActions)
    {
    }


    public override async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        if (!await InvokeFiltersAsync(context, cancellationToken).ConfigureAwait(false))
            return;

        var command = context.Components.Resolve<TCommand>();

        var commandNameRetriever = context.Components.Resolve<ICommandNameRetriever>();

        var commandName = commandNameRetriever.Retrieve(context, command.Id);

        var entityTextManipulator =
            context.Components.Resolve<IEntityTextManipulator<TEntity>>();

        var text = entityTextManipulator.Get(context.Entity);

        if (IsMatches(text, commandName, out var resultText))
        {
            entityTextManipulator.Set(context.Entity, resultText);

            var commandArgumentsParser = context.Components.Resolve<ICommandArgumentsParser<TArguments>>();

            var argumentsText = entityTextManipulator.Get(context.Entity);

            var parseResult =
                commandArgumentsParser.Parse(argumentsText);

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
}