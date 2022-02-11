using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Sessions.Abstractions;

public interface ISession
{
    long OwnerId { get; }
    
    long ChatId { get; }

    bool IsEntityRequested { get; }
}

public interface ISession<TEntityContext, TEntity> : ISession
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    Task WriteEntityAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task<TEntity?> RequestAsync(TEntityContext entityContext, ISessionRequestOptions<TEntityContext, TEntity> options, CancellationToken cancellationToken = default);
    
    Task<TEntity?> RequestAsync(TEntityContext entityContext, Action<ISessionRequestOptionsBuilder<TEntityContext, TEntity>> configureRequest, CancellationToken cancellationToken = default);
}