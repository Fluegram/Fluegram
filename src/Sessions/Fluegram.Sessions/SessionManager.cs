using System.Collections.Concurrent;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public class SessionManager<TEntityContext, TEntity> : ISessionManager<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    private readonly ConcurrentDictionary<SessionDescriptor, ISession<TEntityContext, TEntity>> _sessions;

    public SessionManager()
    {
        _sessions = new ConcurrentDictionary<SessionDescriptor, ISession<TEntityContext, TEntity>>();
    }

    public ISession<TEntityContext, TEntity> GetOrCreate(long ownerId, long chatId)
    {
        var descriptor = new SessionDescriptor(ownerId, chatId);

        if (_sessions.TryGetValue(descriptor, out var session)) return session;

        ISession<TEntityContext, TEntity> newSession = new Session<TEntityContext, TEntity>(ownerId, chatId);

        _sessions.TryAdd(descriptor, newSession);

        return newSession;
    }
}