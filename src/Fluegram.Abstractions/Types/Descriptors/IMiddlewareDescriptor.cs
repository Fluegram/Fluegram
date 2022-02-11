using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Abstractions.Types.Descriptors;

public interface IMiddlewareDescriptor<TEntityContext, TEntity> : ITypedDescriptor
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
{
    
}