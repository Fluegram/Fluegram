namespace Fluegram.Abstractions.Types.Contexts;

public interface IEntityContext<TEntity> : IContext
    where TEntity : class
{
    TEntity Entity { get; }
}