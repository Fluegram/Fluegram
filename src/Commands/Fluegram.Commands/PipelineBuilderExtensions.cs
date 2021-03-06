using System.Reflection;
using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Commands.Abstractions;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Commands.Attributes;
using Fluegram.Commands.Middlewares;
using Fluegram.Commands.Parsing;
using Fluegram.Commands.Types.Descriptors;
using TypeExtensions = Fluegram.Filters.TypeExtensions;

namespace Fluegram.Commands;

public static class PipelineBuilderExtensions
{
    public static IPipelineBuilder<TEntityContext, TEntity> UseCommands<TEntityContext, TEntity>(
        this IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder,
        Action<CommandsPipelineConfigurator<TEntityContext, TEntity>> configure)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        CommandsPipelineConfigurator<TEntityContext, TEntity> configurator = new(pipelineBuilder);

        configure(configurator);

        return pipelineBuilder;
    }

    internal static void RegisterCommandParser<TArguments>(this ContainerBuilder containerBuilder) where TArguments : class, new()
    {
        PropertyInfo[] properties = typeof(TArguments).GetProperties()
            .Where(_ => _.SetMethod is { } && _.GetCustomAttribute<CommandArgumentAttribute>() is { }).ToArray();

        containerBuilder.RegisterType<CommandArgumentsParser<TArguments>>()
            .AsImplementedInterfaces()
            .IfNotRegistered(typeof(ICommandArgumentsParser<TArguments>))
            .WithParameter(new PositionalParameter(0, properties));
    }

    public class CommandsPipelineConfigurator<TEntityContext, TEntity>
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        private readonly IPipelineBuilder<TEntityContext, TEntity> _pipelineBuilder;

        public CommandsPipelineConfigurator(IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder)
        {
            _pipelineBuilder = pipelineBuilder;
        }

        public CommandsPipelineConfigurator<TEntityContext, TEntity> Use<TCommand>()
            where TCommand : ICommand<TEntityContext, TEntity>
        {
            _pipelineBuilder.Components.RegisterType<TCommand>();

            var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
            var preProcessingActions =
                Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
            var postProcessingActions =
                Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();

            _pipelineBuilder.Components.RegisterType<DefaultCommandMiddleware<TCommand, TEntityContext, TEntity>>()
                .WithParameter(new PositionalParameter(0, filters))
                .WithParameter(new PositionalParameter(1, preProcessingActions))
                .WithParameter(new PositionalParameter(2, postProcessingActions));

            _pipelineBuilder.Use<DefaultCommandMiddleware<TCommand, TEntityContext, TEntity>>();

            return this;
        }

        public CommandsPipelineConfigurator<TEntityContext, TEntity> Use<TCommand, TArguments>()
            where TCommand : ICommand<TEntityContext, TEntity, TArguments> where TArguments : class, new()
        {
            _pipelineBuilder.Components.RegisterType<TCommand>();

            _pipelineBuilder.Components.RegisterCommandParser<TArguments>();
            
            var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
            var preProcessingActions =
                Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
            var postProcessingActions =
                Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();

            _pipelineBuilder.Components
                .RegisterType<DefaultCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>()
                .WithParameter(new PositionalParameter(0, filters))
                .WithParameter(new PositionalParameter(1, preProcessingActions))
                .WithParameter(new PositionalParameter(2, postProcessingActions));

            _pipelineBuilder.Use<DefaultCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>();

            return this;
        }


        public CommandsPipelineConfigurator<TEntityContext, TEntity> Use<TCommand>(
            Action<SubCommandsConfigurator> configureSubCommands)
            where TCommand : ICommand<TEntityContext, TEntity>
        {
            _pipelineBuilder.Components.RegisterType<TCommand>();

            var configurator = new SubCommandsConfigurator(_pipelineBuilder.Components);

            configureSubCommands(configurator);

            var descriptors = configurator.Build();

            var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
            var preProcessingActions =
                Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
            var postProcessingActions =
                Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();

            _pipelineBuilder.Components.RegisterType<ComplexCommandMiddleware<TCommand, TEntityContext, TEntity>>()
                .WithParameter(new PositionalParameter(0, filters))
                .WithParameter(new PositionalParameter(1, preProcessingActions))
                .WithParameter(new PositionalParameter(2, postProcessingActions))
                .WithParameter(new PositionalParameter(3, descriptors));

            _pipelineBuilder.Use<ComplexCommandMiddleware<TCommand, TEntityContext, TEntity>>();

            return this;
        }

        public CommandsPipelineConfigurator<TEntityContext, TEntity> Use<TCommand, TArguments>(
            Action<SubCommandsConfigurator> configureSubCommands)
            where TCommand : ICommand<TEntityContext, TEntity, TArguments> where TArguments : class, new()
        {
            _pipelineBuilder.Components.RegisterType<TCommand>();
            
            _pipelineBuilder.Components.RegisterCommandParser<TArguments>();

            var configurator = new SubCommandsConfigurator(_pipelineBuilder.Components);

            configureSubCommands(configurator);

            var descriptors = configurator.Build();

            var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
            var preProcessingActions =
                Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
            var postProcessingActions =
                Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();

            _pipelineBuilder.Components
                .RegisterType<ComplexCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>()
                .WithParameter(new PositionalParameter(0, filters))
                .WithParameter(new PositionalParameter(1, preProcessingActions))
                .WithParameter(new PositionalParameter(2, postProcessingActions))
                .WithParameter(new PositionalParameter(3, descriptors));

            _pipelineBuilder.Use<ComplexCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>();

            return this;
        }


        public class SubCommandsConfigurator : IBuilder<IEnumerable<ChildMiddlewareDescriptor<TEntityContext, TEntity>>>
        {
            private readonly ContainerBuilder _components;

            private readonly List<ChildMiddlewareDescriptor<TEntityContext, TEntity>> _descriptors;

            public SubCommandsConfigurator(ContainerBuilder components)
            {
                _components = components;
                _descriptors = new List<ChildMiddlewareDescriptor<TEntityContext, TEntity>>();
            }


            public IEnumerable<ChildMiddlewareDescriptor<TEntityContext, TEntity>> Build()
            {
                return _descriptors;
            }

            public SubCommandsConfigurator Use<TCommand>()
                where TCommand : ICommand<TEntityContext, TEntity>
            {
                _components.RegisterType<TCommand>();

                var id = Guid.NewGuid();

                var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
                var preProcessingActions =
                    Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
                var postProcessingActions =
                    Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();

                _components.RegisterType<DefaultCommandMiddleware<TCommand, TEntityContext, TEntity>>()
                    .WithParameter(new PositionalParameter(0, filters))
                    .WithParameter(new PositionalParameter(1, preProcessingActions))
                    .WithParameter(new PositionalParameter(2, postProcessingActions))
                    .Keyed<DefaultCommandMiddleware<TCommand, TEntityContext, TEntity>>(id);

                _descriptors.Add(new ChildMiddlewareDescriptor<TEntityContext, TEntity>(ctx => ctx
                    .ResolveKeyed<DefaultCommandMiddleware<TCommand, TEntityContext, TEntity>>(id)));

                return this;
            }

            public SubCommandsConfigurator Use<TCommand, TArguments>()
                where TCommand : ICommand<TEntityContext, TEntity, TArguments> where TArguments : class, new()
            {
                _components.RegisterType<TCommand>();

                _components.RegisterCommandParser<TArguments>();
                
                var id = Guid.NewGuid();

                var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
                var preProcessingActions =
                    Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
                var postProcessingActions =
                    Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();

                _components.RegisterType<DefaultCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>()
                    .WithParameter(new PositionalParameter(0, filters))
                    .WithParameter(new PositionalParameter(1, preProcessingActions))
                    .WithParameter(new PositionalParameter(2, postProcessingActions))
                    .Keyed<DefaultCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>(id);

                _descriptors.Add(new ChildMiddlewareDescriptor<TEntityContext, TEntity>(ctx => ctx
                    .ResolveKeyed<DefaultCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>(id)));

                return this;
            }


            public SubCommandsConfigurator Use<TCommand>(Action<SubCommandsConfigurator> configureSubCommands)
                where TCommand : ICommand<TEntityContext, TEntity>
            {
                _components.RegisterType<TCommand>();

                var configurator = new SubCommandsConfigurator(_components);

                configureSubCommands(configurator);

                var descriptors = configurator.Build();

                var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
                var preProcessingActions =
                    Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
                var postProcessingActions =
                    Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();


                var id = Guid.NewGuid();
                _components.RegisterType<ComplexCommandMiddleware<TCommand, TEntityContext, TEntity>>()
                    .WithParameter(new PositionalParameter(0, filters))
                    .WithParameter(new PositionalParameter(1, preProcessingActions))
                    .WithParameter(new PositionalParameter(2, postProcessingActions))
                    .WithParameter(new PositionalParameter(3, descriptors))
                    .Keyed<ComplexCommandMiddleware<TCommand, TEntityContext, TEntity>>(id);

                _descriptors.Add(new ChildMiddlewareDescriptor<TEntityContext, TEntity>(ctx => ctx
                    .ResolveKeyed<ComplexCommandMiddleware<TCommand, TEntityContext, TEntity>>(id)));

                return this;
            }

            public SubCommandsConfigurator Use<TCommand, TArguments>(
                Action<SubCommandsConfigurator> configureSubCommands)
                where TCommand : ICommand<TEntityContext, TEntity, TArguments> where TArguments : class, new()
            {
                _components.RegisterType<TCommand>();
                
                _components.RegisterCommandParser<TArguments>();

                var configurator = new SubCommandsConfigurator(_components);

                configureSubCommands(configurator);

                var descriptors = configurator.Build();

                var id = Guid.NewGuid();


                var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity, TCommand>();
                var preProcessingActions =
                    Actions.TypeExtensions.FindPreProcessingActionsFor<TEntityContext, TEntity, TCommand>();
                var postProcessingActions =
                    Actions.TypeExtensions.FindPostProcessingActionsFor<TEntityContext, TEntity, TCommand>();


                _components.RegisterType<ComplexCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>()
                    .WithParameter(new PositionalParameter(0, filters))
                    .WithParameter(new PositionalParameter(1, preProcessingActions))
                    .WithParameter(new PositionalParameter(2, postProcessingActions))
                    .WithParameter(new PositionalParameter(3, descriptors))
                    .Keyed<ComplexCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>(id);

                _descriptors.Add(new ChildMiddlewareDescriptor<TEntityContext, TEntity>(ctx => ctx
                    .ResolveKeyed<ComplexCommandMiddleware<TCommand, TArguments, TEntityContext, TEntity>>(id)));

                return this;
            }
        }
    }
}