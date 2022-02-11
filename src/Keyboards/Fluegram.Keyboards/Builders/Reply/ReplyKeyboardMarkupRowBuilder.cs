using Fluegram.Keyboards.Abstractions.Builders.Reply;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Builders.Reply;

public class ReplyKeyboardMarkupRowBuilder : KeyboardMarkupRowBuilderBase<KeyboardButton>, IReplyKeyboardMarkupRowBuilder
{
    public IReplyKeyboardMarkupRowBuilder UseRequestContact(string text)
    {
        Use(KeyboardButton.WithRequestContact(text));

        return this;
    }

    public IReplyKeyboardMarkupRowBuilder UseRequestLocation(string text)
    {
        Use(KeyboardButton.WithRequestLocation(text));

        return this;
    }

    public IReplyKeyboardMarkupRowBuilder UseRequestPoll(string text, string? type = default)
    {
        Use(KeyboardButton.WithRequestPoll(text, type));

        return this;
    }
}