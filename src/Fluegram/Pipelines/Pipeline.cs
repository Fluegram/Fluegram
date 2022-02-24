using Autofac;
using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Abstractions.Types.Descriptors;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Pipelines;

public class Pipeline<TEntityContext, TEntity> : IPipeline<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    private readonly IList<IMiddlewareDescriptor<TEntityContext, TEntity>> _middlewareDescriptors;


    public Pipeline(UpdateType type, IList<IMiddlewareDescriptor<TEntityContext, TEntity>> middlewareDescriptors)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(middlewareDescriptors);

        if (type is UpdateType.Unknown)
        {
        }

        _middlewareDescriptors = middlewareDescriptors;
        Type = type;
    }

    public async Task ProcessEntityAsync(TEntityContext entityContext, CancellationToken cancellationToken)
    {
        foreach (var middlewareDescriptor in _middlewareDescriptors)
        {
            if (entityContext.IsExecutionCancelled) break;

            var middleware = (IMiddleware<TEntityContext, TEntity>)(
                middlewareDescriptor.Name is { } name
                    ? entityContext.Components.ResolveNamed(name, middlewareDescriptor.Type!)
                    : entityContext.Components.Resolve(middlewareDescriptor.Type!));

            try
            {
                await middleware.ProcessAsync(entityContext, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception)
            {
                entityContext.Cancel();
            }
        }
    }

    public UpdateType Type { get; }
}