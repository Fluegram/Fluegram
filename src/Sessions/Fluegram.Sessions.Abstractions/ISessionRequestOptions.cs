using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Sessions.Abstractions;

public interface ISessionRequestOptions<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task>? Action { get; }

    Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task<bool>>? Matcher { get; }

    int? Attempts { get; }
}