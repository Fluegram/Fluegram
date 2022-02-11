using System.Collections.Concurrent;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public record struct SessionDescriptor(long OwnerId, long ChatId);

public class SessionManager<TEntityContext, TEntity> : ISessionManager<TEntityContext, TEntity>

    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    private ConcurrentDictionary<SessionDescriptor, ISession<TEntityContext, TEntity>> _sessions;

    public SessionManager()
    {
        _sessions = new ConcurrentDictionary<SessionDescriptor, ISession<TEntityContext, TEntity>>();
    }

    public ISession<TEntityContext, TEntity> GetOrCreate(long ownerId, long chatId)
    {
        var descriptor = new SessionDescriptor(ownerId, chatId);
        
        if (_sessions.TryGetValue(descriptor, out ISession<TEntityContext, TEntity>? session))
        {
            return session;
        }
        else
        {
            ISession<TEntityContext, TEntity> newSession = new Session<TEntityContext, TEntity>(ownerId, chatId);

            _sessions.TryAdd(descriptor, newSession);

            return newSession;
        }
    }
}