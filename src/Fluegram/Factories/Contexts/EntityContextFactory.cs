using Autofac;
using Fluegram.Abstractions.Factories.Contexts;
using Fluegram.Types.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Fluegram.Factories.Contexts;

public class EntityContextFactory<TEntity> : IEntityContextFactory<EntityContext<TEntity>, TEntity>where TEntity : class
{

    public EntityContext<TEntity> Create(TEntity entity, User? user, Chat? chat, ITelegramBotClient telegramBotClient,
        IComponentContext components)
    {
        return new EntityContext<TEntity>(entity, user, chat, telegramBotClient, components);
    }
}