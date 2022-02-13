using Fluegram.Abstractions.Builders;
using Fluegram.Keyboards.Abstractions.Builders.Inline;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Builders.Inline;

public class InlineKeyboardMarkupRowBuilder : IInlineKeyboardMarkupRowBuilder, IBuilder<List<InlineKeyboardButton>>
{
    private readonly List<InlineKeyboardButton> _buttons;

    public InlineKeyboardMarkupRowBuilder()
    {
        _buttons = new();
    }

    public IInlineKeyboardMarkupRowBuilder UseUrl(string text, string url)
    {
        Use(InlineKeyboardButton.WithUrl(text, url));

        return this;
    }

    public IInlineKeyboardMarkupRowBuilder UseLoginUrl(string text, LoginUrl loginUrl)
    {
        Use(InlineKeyboardButton.WithLoginUrl(text, loginUrl));

        return this;
    }

    public IInlineKeyboardMarkupRowBuilder UseCallbackData(string textAndCallbackData)
    {
        Use(InlineKeyboardButton.WithCallbackData(textAndCallbackData));

        return this;
    }

    public IInlineKeyboardMarkupRowBuilder UseCallbackData(string text, string callbackData)
    {
        Use(InlineKeyboardButton.WithCallbackData(text, callbackData));

        return this;
    }

    public IInlineKeyboardMarkupRowBuilder UseSwitchInlineQuery(string text, string query = "")
    {
        Use(InlineKeyboardButton.WithSwitchInlineQuery(text, query));

        return this;
    }

    public IInlineKeyboardMarkupRowBuilder UseSwitchInlineQueryCurrentChat(string text, string query = "")
    {
        Use(InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(text, query));

        return this;
        
    }

    public IInlineKeyboardMarkupRowBuilder UseCallBackGame(string text, CallbackGame? callbackGame = default)
    {
        Use(InlineKeyboardButton.WithCallBackGame(text, callbackGame));

        return this;
    }

    public IInlineKeyboardMarkupRowBuilder UsePayment(string text)
    {
        Use(InlineKeyboardButton.WithPayment(text));

        return this;
    }

    public IInlineKeyboardMarkupRowBuilder Use(InlineKeyboardButton button)
    {
        _buttons.Add(button);

        return this;
    }

    public List<InlineKeyboardButton> Build()
    {
        return _buttons;
    }
}