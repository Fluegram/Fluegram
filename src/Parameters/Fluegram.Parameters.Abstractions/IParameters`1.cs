using Telegram.Bot;

namespace Fluegram.Parameters.Abstractions;

public interface IParameters<T>
{
    Task<T> InvokeAsync(ITelegramBotClient telegramBotClient, CancellationToken cancellationToken);
}