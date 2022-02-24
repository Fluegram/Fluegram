using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Fluegram.Abstractions.Factories.Contexts;

public interface IEntityContextFactory<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    TEntityContext Create(TEntity entity, User? user, Chat? chat, ITelegramBotClient telegramBotClient,
        IComponentContext components);
}