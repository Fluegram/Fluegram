using Autofac;
using Autofac.Core;
using Fluegram.Abstractions.Builders;
using Fluegram.Builders;

namespace Fluegram;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterFluegram(this ContainerBuilder containerBuilder,
        Action<IFluegramBotBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(containerBuilder);
        ArgumentNullException.ThrowIfNull(configure);
        
        var fluegramBotBuilder = new FluegramBotBuilder(containerBuilder);

        configure(fluegramBotBuilder);

        containerBuilder.Register(ctx => fluegramBotBuilder.Build(ctx.Resolve<IComponentContext>()));

        return containerBuilder;
    }
}