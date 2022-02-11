using Autofac;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Abstractions;

/// <summary>
/// Interface for implementation.
/// </summary>
public interface IFluegramBot
{
    /// <summary>
    /// <see cref="ITelegramBotClient"/> instance.
    /// </summary>
    ITelegramBotClient Client { get; }
    
    /// <summary>
    /// Components context used for service/component resolution.
    /// </summary>
    IComponentContext Components { get; }
    
    /// <summary>
    /// Starts processing an <see cref="Update"/>.
    /// </summary>
    /// <param name="update">Update to be processed.</param>
    /// <param name="cancellationToken">Cancellation token used to interrupt processing.</param>
    /// <exception cref="InvalidOperationException">When <see cref="Update"/>.<see cref="Update.Type"/> is <see cref="UpdateType.Unknown"/>.</exception>
    /// <returns>Processing task.</returns>
    Task ProcessUpdateAsync(Update update, CancellationToken cancellationToken = default);
}