using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Fluegram.Commands.Abstractions;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Commands.Types.Descriptors;
using Fluegram.Filters.Abstractions.Filtering;

namespace Fluegram.Commands.Middlewares;

public class ComplexCommandMiddleware<TCommand, TEntityContext, TEntity> : CommandMiddlewareBase<TEntityContext, TEntity> where TEntityContext : IEntityContext<TEntity> where TEntity : class
    where TCommand : ICommand<TEntityContext, TEntity>
{
    private readonly IEnumerable<ChildMiddlewareDescriptor<TEntityContext, TEntity>> _childMiddlewareDescriptors;

    

    public override async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        if (!await InvokeFiltersAsync(context, cancellationToken).ConfigureAwait(false))
            return;

        TCommand command = context.Components.Resolve<TCommand>();

        ICommandNameRetriever commandNameRetriever = context.Components.Resolve<ICommandNameRetriever>();

        string commandName = commandNameRetriever.Retrieve(context, command.Id);

        IEntityTextManipulator<TEntity> entityTextManipulator = context.Components.Resolve<IEntityTextManipulator<TEntity>>();

        string text = entityTextManipulator.Get(context.Entity);

        if (IsMatches(text, commandName,  out var resultText))
        {
            entityTextManipulator.Set(context.Entity, resultText);

            bool subCommandExecuted = false;
            
            foreach (var descriptor in _childMiddlewareDescriptors)
            {
                var middleware = descriptor.MiddlewareResolver(context.Components);

                await middleware.ProcessAsync(context, cancellationToken).ConfigureAwait(false);
                
                if (context.IsExecutionCancelled)
                {
                    subCommandExecuted = true;
                    break;
                }
            }

            if (!subCommandExecuted)
            {
                await command.ProcessAsync(context, cancellationToken).ConfigureAwait(false);
                
                context.Cancel();
            }
        }
    }

    public ComplexCommandMiddleware(
        IEnumerable<IFilter<TEntityContext, TEntity>> filters, 
        IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions, 
        IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions,
        IEnumerable<ChildMiddlewareDescriptor<TEntityContext, TEntity>> childMiddlewareDescriptors) : base(filters, preProcessingActions, postProcessingActions)
    {
        _childMiddlewareDescriptors = childMiddlewareDescriptors;
    }
}


public class ComplexCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity> : CommandMiddlewareBase<TEntityContext, TEntity> where TEntityContext : IEntityContext<TEntity> where TEntity : class
    where TCommand : ICommand<TEntityContext, TEntity, TArguments>
    where TArguments : class, new()
{
    private readonly IEnumerable<ChildMiddlewareDescriptor<TEntityContext, TEntity>> _childMiddlewareDescriptors;


    public override async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        if (!await InvokeFiltersAsync(context, cancellationToken).ConfigureAwait(false))
            return;

        TCommand command = context.Components.Resolve<TCommand>();

        ICommandNameRetriever commandNameRetriever = context.Components.Resolve<ICommandNameRetriever>();

        string commandName = commandNameRetriever.Retrieve(context, command.Id);

        IEntityTextManipulator<TEntity> entityTextManipulator = context.Components.Resolve<IEntityTextManipulator<TEntity>>();

        string text = entityTextManipulator.Get(context.Entity);

        if (IsMatches(text, commandName,  out var resultText))
        {
            entityTextManipulator.Set(context.Entity, resultText);

            bool subCommandExecuted = false;
            
            foreach (var descriptor in _childMiddlewareDescriptors)
            {
                if (context.IsExecutionCancelled)
                {
                    subCommandExecuted = true;
                    break;
                }

                var middleware = descriptor.MiddlewareResolver(context.Components);

                await middleware.ProcessAsync(context, cancellationToken).ConfigureAwait(false);
            }

            if (!subCommandExecuted)
            {
                ICommandArgumentsParser commandArgumentsParser = context.Components.Resolve<ICommandArgumentsParser>();

                string argumentsText = entityTextManipulator.Get(context.Entity);
            
                ICommandArgumentsParseResult<TArguments> parseResult = commandArgumentsParser.Parse<TArguments>(argumentsText);

                if (parseResult is ICommandArgumentsSuccessfulParseResult<TArguments> { Arguments: { } arguments })
                {
                    await command.ProcessAsync(context, arguments, cancellationToken).ConfigureAwait(false);
                    
                    context.Cancel();
                }
                else if(parseResult is ICommandArgumentsFailedParseResult<TArguments> { Errors: {} errors})
                {
                    await command.ProcessInvalidArgumentsAsync(context, errors, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }

    public ComplexCommandMiddleware(IEnumerable<IFilter<TEntityContext, TEntity>> filters, IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions, IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions, IEnumerable<ChildMiddlewareDescriptor<TEntityContext, TEntity>> childMiddlewareDescriptors) : base(filters, preProcessingActions, postProcessingActions)
    {
        _childMiddlewareDescriptors = childMiddlewareDescriptors;
    }
}


