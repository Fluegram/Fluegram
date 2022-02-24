using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Fluegram.Types.Contexts;

public class EntityContext<TEntity> : IEntityContext<TEntity> where TEntity : class
{
    public EntityContext(TEntity entity, User? user, Chat? chat, ITelegramBotClient client,
        IComponentContext components)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(components);

        Entity = entity;
        User = user;
        Chat = chat;
        Client = client;
        Components = components;
    }

    public EntityContext(TEntity entity, IContext context) : this(entity, context.User, context.Chat, context.Client,
        context.Components)
    {
    }

    public TEntity Entity { get; }

    public bool IsExecutionCancelled { get; private set; }

    public void Cancel()
    {
        IsExecutionCancelled = true;
    }

    public User? User { get; }
    public Chat? Chat { get; }
    public ITelegramBotClient Client { get; }

    public IComponentContext Components { get; }
}