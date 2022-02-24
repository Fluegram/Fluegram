using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Abstractions;

public interface ICommand<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    string Id { get; }

    Task ProcessAsync(TEntityContext entityContext, CancellationToken cancellationToken);
}

public interface ICommand<TEntityContext, TEntity, TArguments>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
    where TArguments : class, new()
{
    string Id { get; }

    Task ProcessAsync(TEntityContext entityContext, TArguments arguments, CancellationToken cancellationToken);

    Task ProcessInvalidArgumentsAsync(TEntityContext entityContext,
        IEnumerable<ICommandArgumentParseError> argumentsParseErrors, CancellationToken cancellationToken);
}