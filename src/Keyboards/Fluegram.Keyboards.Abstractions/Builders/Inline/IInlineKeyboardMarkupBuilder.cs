using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Abstractions.Builders.Inline;

public interface IInlineKeyboardMarkupBuilder : IKeyboardMarkupBuilder<InlineKeyboardMarkup, IInlineKeyboardMarkupRowBuilder, InlineKeyboardButton>
{
    
}