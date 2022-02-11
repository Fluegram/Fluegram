using System.Threading.Channels;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Sessions.Abstractions;

namespace Fluegram.Sessions;

public class Session<TEntityContext, TEntity> : ISession<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    private readonly ChannelWriter<TEntity> _entityWriter;

    private readonly ChannelReader<TEntity> _entityReader;

    public long OwnerId { get; }
    public long ChatId { get; }

    public bool IsEntityRequested { get; private set; }
    

    public Session(long ownerId, long chatId)
    {
        OwnerId = ownerId;
        ChatId = chatId;

        var channel = Channel.CreateBounded<TEntity>(new BoundedChannelOptions(1)
        {
            AllowSynchronousContinuations = true,
            Capacity = 1,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = true
        });

        _entityWriter = channel.Writer;
        _entityReader = channel.Reader;
    }


    public async Task WriteEntityAsync(TEntity entity, CancellationToken cancellationToken) => await _entityWriter.WriteAsync(entity, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async Task<TEntity?> RequestAsync(TEntityContext entityContext, ISessionRequestOptions<TEntityContext, TEntity> options, CancellationToken cancellationToken = default)
    {
        IsEntityRequested = true;
        
        int currentAttempt = 0;

        while (IsEntityRequested
            && currentAttempt != (options.Attempts ?? -1) 
               && !cancellationToken.IsCancellationRequested)
        {
            var state = new SessionRequestState<TEntity>(null, currentAttempt, options.Attempts - currentAttempt);

            await options.Action?.Invoke(state, entityContext, cancellationToken)!;

            TEntity entity = await _entityReader.ReadAsync(cancellationToken);

            state = new SessionRequestState<TEntity>(entity, currentAttempt, options.Attempts - currentAttempt);

            bool matches = true;

            if (options.Matcher is { })
                matches = await options.Matcher(state, entityContext, cancellationToken).ConfigureAwait(false);

            if (matches)
            {
                IsEntityRequested = false;
                
                return entity;
            }

            currentAttempt++;
        }

        return default;
    }

    public Task<TEntity?> RequestAsync(TEntityContext entityContext, Action<ISessionRequestOptionsBuilder<TEntityContext, TEntity>> configureRequest, CancellationToken cancellationToken = default)
    {
        ISessionRequestOptionsBuilder<TEntityContext, TEntity> optionsBuilder = new SessionRequestOptionsBuilder<TEntityContext, TEntity>();

        configureRequest(optionsBuilder);
        
        return RequestAsync(entityContext, optionsBuilder.Build(), cancellationToken);
    }
}