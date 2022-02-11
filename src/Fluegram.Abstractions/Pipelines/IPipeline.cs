using Fluegram.Abstractions.Types.Contexts;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Abstractions.Pipelines;

public interface IPipeline
{
    UpdateType Type { get; }
}

public interface IPipeline<TEntity> : IPipeline
{
    
}

public interface IPipeline<TEntityContext, TEntity> : IPipeline<TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    Task ProcessEntityAsync(TEntityContext entityContext, CancellationToken cancellationToken);
}