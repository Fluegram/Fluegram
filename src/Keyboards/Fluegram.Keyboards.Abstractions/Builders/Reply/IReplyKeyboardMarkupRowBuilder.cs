using Fluegram.Abstractions.Builders;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Abstractions.Builders.Reply;

public interface IReplyKeyboardMarkupRowBuilder : IBuilder<List<KeyboardButton>>
{
    IReplyKeyboardMarkupRowBuilder UseRequestContact(string text);

    IReplyKeyboardMarkupRowBuilder UseRequestLocation(string text);

    IReplyKeyboardMarkupRowBuilder UseRequestPoll(string text, string? type = default);
    
    IReplyKeyboardMarkupRowBuilder Use(KeyboardButton button);
}