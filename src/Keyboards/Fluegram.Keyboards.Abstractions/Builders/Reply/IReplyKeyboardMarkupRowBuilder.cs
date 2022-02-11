using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Abstractions.Builders.Reply;

public interface IReplyKeyboardMarkupRowBuilder : IKeyboardMarkupRowBuilder<KeyboardButton>
{
    IReplyKeyboardMarkupRowBuilder UseRequestContact(string text);

    IReplyKeyboardMarkupRowBuilder UseRequestLocation(string text);

    IReplyKeyboardMarkupRowBuilder UseRequestPoll(string text, string? type = default);
}