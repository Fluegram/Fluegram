using Autofac;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Fluegram.Abstractions.Types.Contexts;

public interface IContext 
{
    User? User { get; }
    
    Chat? Chat { get; }
    
    ITelegramBotClient Client { get; }
    
    IComponentContext Components { get; }
    
    bool IsExecutionCancelled { get; }
    
    void Cancel();
}

public interface IEntityContext<TEntity> : IContext
    where TEntity : class
{
    TEntity Entity { get; }
}