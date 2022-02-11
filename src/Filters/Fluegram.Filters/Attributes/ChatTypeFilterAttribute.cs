using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Filters.Abstractions.Filtering;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Filters.Attributes;

public class ChatTypeFilterAttribute<TEntityContext, TEntity> : IFilter<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public ChatTypeFilterAttribute(ChatType type)
    {
        Type = type;
    }

    public ChatType Type { get; }

    public Task<bool> MatchesAsync(TEntityContext entityContext, CancellationToken cancellationToken)
    {
        return Task.FromResult(entityContext.Chat!.Type == Type);
    }
}