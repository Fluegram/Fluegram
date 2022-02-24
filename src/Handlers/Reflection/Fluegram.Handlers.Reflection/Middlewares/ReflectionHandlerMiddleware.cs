using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Handlers.Reflection.Abstractions.Middlewares;
using Fluegram.Handlers.Reflection.Abstractions.Types.Descriptors;

namespace Fluegram.Handlers.Reflection.Middlewares;

public class
    ReflectionHandlerMiddleware<TEntityContext, TEntity> : IReflectionHandlerMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    public ReflectionHandlerMiddleware(IReflectionHandlerDescriptor<TEntityContext, TEntity> descriptor)
    {
        Descriptor = descriptor;
    }

    public IReflectionHandlerDescriptor<TEntityContext, TEntity> Descriptor { get; }

    public async Task ProcessAsync(TEntityContext context, CancellationToken cancellationToken)
    {
        var arguments = RetrieveArguments(context, cancellationToken);

        object? declaringTypeInstance = null;

        var declaringType = Descriptor.Type;
        if (!declaringType!.IsAbstract && !declaringType.IsSealed)
            declaringTypeInstance = context.Components.Resolve(declaringType);

        foreach (var filter in Descriptor.Filters)
            if (!await filter.MatchesAsync(context, cancellationToken).ConfigureAwait(false))
                return;

        foreach (var preProcessingAction in Descriptor.PreProcessingActions)
            await preProcessingAction.InvokeAsync(context, cancellationToken).ConfigureAwait(false);

        await ((Task)Descriptor.Method.Invoke(declaringTypeInstance, arguments)!).ConfigureAwait(false);

        foreach (var postProcessingAction in Descriptor.PreProcessingActions)
            await postProcessingAction.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
    }

    private object[] RetrieveArguments(TEntityContext entityContext, CancellationToken cancellationToken)
    {
        var parameters = Descriptor.Method.GetParameters();

        var arguments = new object[parameters.Length];

        for (var i = 0; i < arguments.Length; i++)
        {
            object argumentValue;

            var parameterInfo = parameters[i];

            var argumentType = parameterInfo.ParameterType;

            if (argumentType == typeof(TEntityContext))
            {
                argumentValue = entityContext;
            }
            else if (argumentType == typeof(CancellationToken))
            {
                argumentValue = cancellationToken;
            }
            else
            {
                if (parameterInfo.IsOptional)
                    argumentValue = entityContext.Components.ResolveOptional(argumentType)!;
                else
                    argumentValue = entityContext.Components.Resolve(argumentType)!;
            }

            arguments[i] = argumentValue;
        }

        return arguments;
    }
}