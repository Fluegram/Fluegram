using Telegram.Bot;
using Telegram.Bot.Types;

namespace Fluegram.Example.Console.Services;

public class BotInformationService
{
    private readonly ITelegramBotClient _telegramBotClient;

    public BotInformationService(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    
    public User Bot { get; private set; }

    public async Task InitializeAsync()
    {
        Bot = await _telegramBotClient.GetMeAsync();
    }
}