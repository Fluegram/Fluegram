using Fluegram.Abstractions.Builders;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Fluegram.Keyboards.Abstractions.Builders.Inline;

public interface IInlineKeyboardMarkupRowBuilder : IBuilder<List<InlineKeyboardButton>>
{
    IInlineKeyboardMarkupRowBuilder UseUrl(string text, string url);

    IInlineKeyboardMarkupRowBuilder UseLoginUrl(string text, LoginUrl loginUrl);

    IInlineKeyboardMarkupRowBuilder UseCallbackData(string textAndCallbackData);

    IInlineKeyboardMarkupRowBuilder UseCallbackData(string text, string callbackData);

    IInlineKeyboardMarkupRowBuilder UseSwitchInlineQuery(string text, string query = "");

    IInlineKeyboardMarkupRowBuilder UseSwitchInlineQueryCurrentChat(string text, string query = "");

    IInlineKeyboardMarkupRowBuilder UseCallBackGame(string text, CallbackGame? callbackGame = default);

    IInlineKeyboardMarkupRowBuilder UsePayment(string text);

    IInlineKeyboardMarkupRowBuilder Use(InlineKeyboardButton button);
}