using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public static class PipelineFeaturesConfiguratorExtensions
{
    public static IPipelineFeaturesConfigurator<TEntityContext, TEntity> UseSessionManagement<TEntityContext, TEntity>(
        this IPipelineFeaturesConfigurator<TEntityContext, TEntity> featuresConfigurator)
        where TEntityContext : IEntityContext<TEntity>
        where TEntity : class
    {
        featuresConfigurator.Components.RegisterType<SessionManager<TEntityContext, TEntity>>()
            .IfNotRegistered(typeof(ISessionManager<TEntityContext, TEntity>))
            .As<ISessionManager<TEntityContext, TEntity>>()
            .SingleInstance();

        return featuresConfigurator;
    }
}