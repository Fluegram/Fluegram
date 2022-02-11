using Telegram.Bot;

namespace Fluegram.Parameters.Abstractions;

public interface IParameters
{
    Task InvokeAsync(ITelegramBotClient telegramBotClient, CancellationToken cancellationToken);
}