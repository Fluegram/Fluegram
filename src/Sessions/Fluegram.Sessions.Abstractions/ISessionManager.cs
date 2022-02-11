using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Sessions.Abstractions;

public interface ISessionManager<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    ISession<TEntityContext, TEntity> GetOrCreate(long ownerId, long chatId);

}