using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Handlers;
using Fluegram.Handlers.Abstractions;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public static class PipelineBuilderExtensions
{
    public static IPipelineBuilder<TEntityContext, TEntity> UseSession<TEntityContext, TEntity>(
        this IPipelineBuilder<TEntityContext, TEntity> pipelineBuilder)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        pipelineBuilder.UseHandlers(_ => _
            .Use<SessionHandler<TEntityContext, TEntity>>()
        );

        return pipelineBuilder;
    }

    public class SessionHandler<TEntityContext, TEntity> : IHandler<TEntityContext, TEntity>
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        private readonly ISessionManager<TEntityContext, TEntity> _sessionManager;

        public SessionHandler(ISessionManager<TEntityContext, TEntity> sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task HandleAsync(TEntityContext entityContext, CancellationToken cancellationToken)
        {
            var session = _sessionManager.GetOrCreate(entityContext.User!.Id, entityContext.Chat!.Id);

            if (entityContext.Chat.Id == session.ChatId && session.IsEntityRequested)
            {
                await session.WriteEntityAsync(entityContext.Entity, cancellationToken).ConfigureAwait(false);

                entityContext.Cancel();
            }
        }
    }
}