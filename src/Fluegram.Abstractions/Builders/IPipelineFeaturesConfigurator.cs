using Autofac;
using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Abstractions.Builders;

public interface IPipelineFeaturesConfigurator<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    ContainerBuilder Components { get; }
}