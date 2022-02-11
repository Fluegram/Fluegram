using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public class SessionRequestOptions<TEntityContext, TEntity> : ISessionRequestOptions<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public SessionRequestOptions(Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task>? action,
        Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task<bool>>? matcher, int attempts = -1)
    {
        Action = action;
        Matcher = matcher;
        Attempts = attempts;
    }

    public Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task>? Action { get; }
    public Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task<bool>>? Matcher { get; }
    public int? Attempts { get; }
}