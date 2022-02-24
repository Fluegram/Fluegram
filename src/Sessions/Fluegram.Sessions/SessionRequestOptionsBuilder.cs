using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public class
    SessionRequestOptionsBuilder<TEntityContext, TEntity> : ISessionRequestOptionsBuilder<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    private Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task>? _action;
    private int _attempts = -1;
    private Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task<bool>>? _matcher;

    public ISessionRequestOptions<TEntityContext, TEntity> Build()
    {
        return new SessionRequestOptions<TEntityContext, TEntity>(_action, _matcher, _attempts);
    }

    public ISessionRequestOptionsBuilder<TEntityContext, TEntity> UseAction(
        Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task> action)
    {
        _action = action;

        return this;
    }

    public ISessionRequestOptionsBuilder<TEntityContext, TEntity> UseMatcher(
        Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task<bool>> matcher)
    {
        _matcher = matcher;

        return this;
    }

    public ISessionRequestOptionsBuilder<TEntityContext, TEntity> UseAttempts(int attempts)
    {
        if (attempts < 1) throw new ArgumentException("Attempts count should be greater or equals to 1");

        _attempts = attempts;

        return this;
    }
}