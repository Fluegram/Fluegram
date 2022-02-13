using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Actions.Attributes;

public class ChatActionAttribute<TEntityContext, TEntity> : Attribute, IPreProcessingAction<TEntityContext, TEntity> where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public ChatActionAttribute(ChatAction action)
    {
        Action = action;
    }

    public ChatAction Action { get; }

    public Task InvokeAsync(TEntityContext entityContext, CancellationToken cancellationToken)
    {
        return entityContext.Client.SendChatActionAsync(entityContext.Chat!, Action, cancellationToken);
    }
}