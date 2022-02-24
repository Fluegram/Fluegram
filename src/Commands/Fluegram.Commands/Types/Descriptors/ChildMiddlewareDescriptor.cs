using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Commands.Abstractions.Middlewares;

namespace Fluegram.Commands.Types.Descriptors;

public record struct ChildMiddlewareDescriptor<TEntityContext, TEntity>(
    Func<IComponentContext, ICommandMiddleware<TEntityContext, TEntity>> MiddlewareResolver)
    where TEntityContext : IEntityContext<TEntity> where TEntity : class;