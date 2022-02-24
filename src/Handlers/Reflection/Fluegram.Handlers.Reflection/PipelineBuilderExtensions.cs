using System.Linq.Expressions;
using System.Reflection;
using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Handlers.Reflection.Middlewares;
using Fluegram.Handlers.Reflection.Types.Descriptors;
using Fluegram.Types.Descriptors;

namespace Fluegram.Handlers.Reflection;

public static class PipelineBuilderExtensions
{
    public static IPipelineBuilder<TEntityContext, TEntity> UseReflectionHandlers<TEntityContext, TEntity>(
        this IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder,
        Action<ReflectionHandlersConfigurator<TEntityContext, TEntity>> configureHandlers)
        where TEntityContext : IEntityContext<TEntity>
        where TEntity : class
    {
        ReflectionHandlersConfigurator<TEntityContext, TEntity> handlersConfigurator = new(pipelineBuilder);

        configureHandlers(handlersConfigurator);

        return pipelineBuilder;
    }

    public class ReflectionHandlersConfigurator<TEntityContext, TEntity>
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        private readonly IPipelineBuilder<TEntityContext, TEntity> _pipelineBuilder;

        internal ReflectionHandlersConfigurator(IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder)
        {
            _pipelineBuilder = pipelineBuilder;
        }

        public ReflectionHandlersConfigurator<TEntityContext, TEntity> UseFromType<T>(
            params Expression<Func<T, Delegate>>[] selectors) where T : class
        {
            var handlers = new List<MethodInfo>();

            if (selectors is { Length: > 0 })
            {
                foreach (var selector in selectors)
                    if (selector.Compile()(default!).Method is { ReturnType: { } returnType } method &&
                        returnType == typeof(Task))
                        handlers.Add(method);
            }
            else
            {
                foreach (var method in typeof(T).GetRuntimeMethods())
                    if (method.IsTaskMethod())
                        handlers.Add(method);
            }

            if (typeof(T).IsInstantiableType())
                _pipelineBuilder.Components.RegisterType(typeof(T));

            foreach (var handler in handlers)
            {
                var name = Guid.NewGuid().ToString();

                var descriptor =
                    ReflectionHandlerDescriptor<TEntityContext, TEntity>.CreateFromMethodInfo(handler);

                _pipelineBuilder.Components.RegisterType<ReflectionHandlerMiddleware<TEntityContext, TEntity>>()
                    .WithParameter(new PositionalParameter(0, descriptor))
                    .Named<ReflectionHandlerMiddleware<TEntityContext, TEntity>>(name);

                _pipelineBuilder.UseDescriptor(MiddlewareDescriptor<TEntityContext, TEntity>
                    .CreateFromMiddleware<ReflectionHandlerMiddleware<TEntityContext, TEntity>>(name));
            }

            return this;
        }
    }
}