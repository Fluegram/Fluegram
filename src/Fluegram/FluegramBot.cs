using Autofac;
using Fluegram.Abstractions;
using Fluegram.Abstractions.Factories.Contexts;
using Fluegram.Abstractions.Pipelines;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Fluegram;

public class FluegramBot : IFluegramBot
{
    private readonly IReadOnlyDictionary<UpdateType, IPipeline> _pipelines;


    public FluegramBot(ITelegramBotClient client, IComponentContext components,
        IReadOnlyDictionary<UpdateType, IPipeline> pipelines)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(components);
        ArgumentNullException.ThrowIfNull(pipelines);

        Client = client;
        Components = components;
        _pipelines = pipelines;
    }

    public ITelegramBotClient Client { get; }
    public IComponentContext Components { get; }


    public Task ProcessUpdateAsync(Update update, CancellationToken cancellationToken = default)
    {
        var user = update.GetUserFromUpdate();
        var chat = update.GetChatFromUpdate();

        var entity = update.GetEntityFromUpdate();

        if (entity is { })
            return (Task)this.GetMethod(nameof(ProcessEntityAsync))!.MakeGenericMethod(entity.GetType()).Invoke(this,
                new[]
                {
                    update,
                    entity,
                    user,
                    chat,
                    cancellationToken
                })!;
        throw new InvalidOperationException("Cannot process update because it has Unknown type.");
    }

    private Task ProcessEntityAsync<TEntity>(Update update, TEntity entity, User? user, Chat? chat,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        if (_pipelines.TryGetValue(update.Type, out var pipeline) && pipeline is IPipeline<TEntity>)
        {
            var entityContextType = pipeline.GetType().GetGenericArguments()[0];

            var processMethod = this.GetMethod(nameof(ProcessEntityWithContextAsync))!.MakeGenericMethod(
                entityContextType,
                entity.GetType());

            return (Task)processMethod.Invoke(this, new object?[]
            {
                pipeline,
                entity,
                user,
                chat,
                cancellationToken
            })!;
        }

        return Task.CompletedTask;
    }

    private Task ProcessEntityWithContextAsync<TEntityContext, TEntity>(IPipeline<TEntityContext, TEntity> pipeline,
        TEntity entity, User? user, Chat? chat, CancellationToken cancellationToken)
        where TEntityContext : IEntityContext<TEntity>
        where TEntity : class
    {
        var entityContextFactory =
            Components.Resolve<IEntityContextFactory<TEntityContext, TEntity>>();

        var entityContext = entityContextFactory.Create(entity, user, chat, Client, Components);

        return pipeline.ProcessEntityAsync(entityContext, cancellationToken);
    }
}