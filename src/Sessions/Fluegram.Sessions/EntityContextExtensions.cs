using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public static class EntityContextExtensions
{
    public static ISession<TEntityContext, TEntity> Session<TEntityContext, TEntity>(this IContext entityContext)
        where TEntityContext : IEntityContext<TEntity>
        where TEntity : class
    {
        var manager = entityContext.Components.Resolve<ISessionManager<TEntityContext, TEntity>>();

        return manager.GetOrCreate(entityContext.User!.Id, entityContext.Chat!.Id);
    }
}