using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Handlers.Reflection.Abstractions.Types.Descriptors;

namespace Fluegram.Handlers.Reflection.Abstractions.Middlewares;

public interface IReflectionHandlerMiddleware<TEntityContext, TEntity> : IMiddleware<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> 
    where TEntity : class
{
    IReflectionHandlerDescriptor<TEntityContext, TEntity> Descriptor { get; }
}