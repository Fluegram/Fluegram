using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public struct SessionRequestState<TEntity> : ISessionRequestState<TEntity>
    where TEntity : class
{
    public SessionRequestState(TEntity? entity, int attempt, int? attemptsLeft)
    {
        Entity = entity;
        Attempt = attempt;
        AttemptsLeft = attemptsLeft;
    }

    public TEntity? Entity { get; }
    public int? Attempt { get; }
    public int? AttemptsLeft { get; }
}