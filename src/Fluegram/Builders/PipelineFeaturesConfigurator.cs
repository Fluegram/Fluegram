using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Builders;

public class
    PipelineFeaturesConfigurator<TEntityContext, TEntity> : IPipelineFeaturesConfigurator<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public PipelineFeaturesConfigurator(ContainerBuilder components)
    {
        Components = components;
    }

    public ContainerBuilder Components { get; }
}