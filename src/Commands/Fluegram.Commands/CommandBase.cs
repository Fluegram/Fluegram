using Fluegram.Commands.Abstractions;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Types.Contexts;

namespace Fluegram.Commands;

public abstract class CommandBase<TEntity> : ICommand<EntityContext<TEntity>, TEntity> where TEntity : class
{
    protected CommandBase(string id)
    {
        Id = id;
    }

    public string Id { get; }
    public abstract Task ProcessAsync(EntityContext<TEntity> entityContext, CancellationToken cancellationToken);
}

public abstract class CommandBase<TEntity, TArguments> : ICommand<EntityContext<TEntity>, TEntity, TArguments>
    where TEntity : class where TArguments : class, new()
{
    protected CommandBase(string id)
    {
        Id = id;
    }

    public string Id { get; }

    public abstract Task ProcessAsync(EntityContext<TEntity> entityContext, TArguments arguments,
        CancellationToken cancellationToken);

    public abstract Task ProcessInvalidArgumentsAsync(EntityContext<TEntity> entityContext,
        IEnumerable<ICommandArgumentParseError> argumentsParseErrors, CancellationToken cancellationToken);
}