using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Abstractions.Types.Descriptors;

namespace Fluegram.Types.Descriptors;

public struct MiddlewareDescriptor<TEntityContext, TEntity> : IMiddlewareDescriptor<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public static MiddlewareDescriptor<TEntityContext, TEntity> CreateFromMiddleware<TMiddleware>(string? key = null)
        where TMiddleware : IMiddleware<TEntityContext, TEntity>
    {
        return new MiddlewareDescriptor<TEntityContext, TEntity>(typeof(TMiddleware), key);
    }

    private MiddlewareDescriptor(Type? type, string? key)
    {
        Type = type;
        Name = key;
    }

    public Type? Type { get; }
    public string? Name { get; }
}