using Fluegram.Keyboards.Abstractions.Builders.Inline;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Builders.Inline;

public class InlineKeyboardMarkupBuilder :
    KeyboardMarkupBuilderBase<InlineKeyboardMarkup, IInlineKeyboardMarkupRowBuilder, InlineKeyboardButton>,
    IInlineKeyboardMarkupBuilder
{
    protected override InlineKeyboardMarkup Build(IEnumerable<IEnumerable<InlineKeyboardButton>> buttons)
    {
        return new InlineKeyboardMarkup(buttons);
    }

    protected override IInlineKeyboardMarkupRowBuilder CreateRowBuilder()
    {
        return new InlineKeyboardMarkupRowBuilder();
    }
}