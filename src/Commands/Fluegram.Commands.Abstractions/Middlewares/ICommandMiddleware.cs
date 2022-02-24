using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Fluegram.Filters.Abstractions.Filtering;

namespace Fluegram.Commands.Abstractions.Middlewares;

public interface ICommandMiddleware<TEntityContext, TEntity> : IMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    IEnumerable<IFilter<TEntityContext, TEntity>> Filters { get; }

    IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> PreProcessingActions { get; }

    IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> PostProcessingActions { get; }
}

public interface ICommandMiddleware<TCommand, TEntityContext, TEntity> : ICommandMiddleware<TEntityContext, TEntity>
    where TCommand : ICommand<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
}

public interface
    ICommandMiddleware<TCommand, TArguments, TEntityContext, TEntity> : ICommandMiddleware<TEntityContext, TEntity>
    where TCommand : ICommand<TEntityContext, TEntity, TArguments>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
    where TArguments : class, new()
{
}