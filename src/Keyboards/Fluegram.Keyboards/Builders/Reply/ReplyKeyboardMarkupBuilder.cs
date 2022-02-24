using Fluegram.Keyboards.Abstractions.Builders.Reply;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Builders.Reply;

public class ReplyKeyboardMarkupBuilder :
    KeyboardMarkupBuilderBase<ReplyKeyboardMarkup, IReplyKeyboardMarkupRowBuilder, KeyboardButton>,
    IReplyKeyboardMarkupBuilder
{
    protected override ReplyKeyboardMarkup Build(IEnumerable<IEnumerable<KeyboardButton>> buttons)
    {
        return new ReplyKeyboardMarkup(buttons);
    }

    protected override IReplyKeyboardMarkupRowBuilder CreateRowBuilder()
    {
        return new ReplyKeyboardMarkupRowBuilder();
    }
}