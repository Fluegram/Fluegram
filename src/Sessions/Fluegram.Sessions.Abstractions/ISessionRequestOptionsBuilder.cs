using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Sessions.Abstractions;

public interface
    ISessionRequestOptionsBuilder<TEntityContext, TEntity> : IBuilder<ISessionRequestOptions<TEntityContext, TEntity>>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    ISessionRequestOptionsBuilder<TEntityContext, TEntity> UseAction(
        Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task> action);

    ISessionRequestOptionsBuilder<TEntityContext, TEntity> UseMatcher(
        Func<ISessionRequestState<TEntity>, TEntityContext, CancellationToken, Task<bool>> matcher);

    ISessionRequestOptionsBuilder<TEntityContext, TEntity> UseAttempts(int attempts);
}