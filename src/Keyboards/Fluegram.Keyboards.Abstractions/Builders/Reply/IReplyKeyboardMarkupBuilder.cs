using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Abstractions.Builders.Reply;

public interface
    IReplyKeyboardMarkupBuilder : IKeyboardMarkupBuilder<ReplyKeyboardMarkup, IReplyKeyboardMarkupRowBuilder,
        KeyboardButton>
{
}