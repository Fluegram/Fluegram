namespace Fluegram.Sessions.Abstractions;

public interface ISessionRequestState<TEntity>
    where TEntity : class
{
    TEntity? Entity { get; }

    int? Attempt { get; }

    int? AttemptsLeft { get; }
}