using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Extensions;

public static class UpdateExtensions
{
    public static object? GetEntityFromUpdate(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message,
            UpdateType.InlineQuery => update.InlineQuery,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult,
            UpdateType.CallbackQuery => update.CallbackQuery,
            UpdateType.EditedMessage => update.EditedMessage,
            UpdateType.ChannelPost => update.ChannelPost,
            UpdateType.EditedChannelPost => update.EditedChannelPost,
            UpdateType.ShippingQuery => update.ShippingQuery,
            UpdateType.PreCheckoutQuery => update.PreCheckoutQuery,
            UpdateType.Poll => update.Poll,
            UpdateType.PollAnswer => update.PollAnswer,
            UpdateType.MyChatMember => update.MyChatMember,
            UpdateType.ChatMember => update.ChatMember,
            UpdateType.ChatJoinRequest => update.ChatJoinRequest,
            UpdateType.Unknown => null,
            _ => null
        };
    }

    public static User? GetUserFromUpdate(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Unknown => null,
            UpdateType.Message => update.Message!.From,
            UpdateType.InlineQuery => update.InlineQuery!.From,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult!.From,
            UpdateType.CallbackQuery => update.CallbackQuery!.From,
            UpdateType.EditedMessage => update.EditedMessage!.From,
            UpdateType.ChannelPost => update.ChannelPost!.From,
            UpdateType.EditedChannelPost => update.EditedChannelPost!.From,
            UpdateType.ShippingQuery => update.ShippingQuery!.From,
            UpdateType.PreCheckoutQuery => update.PreCheckoutQuery!.From,
            UpdateType.Poll => null,
            UpdateType.PollAnswer => update.PollAnswer!.User,
            UpdateType.MyChatMember => update.MyChatMember!.From,
            UpdateType.ChatMember => update.ChatMember!.From,
            UpdateType.ChatJoinRequest => update.ChatJoinRequest!.From,
            _ => null
        };
    }

    public static Chat? GetChatFromUpdate(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Unknown => null,
            UpdateType.Message => update.Message!.Chat,
            UpdateType.InlineQuery => null,
            UpdateType.ChosenInlineResult => null,
            UpdateType.CallbackQuery => update.CallbackQuery!.Message!.Chat,
            UpdateType.EditedMessage => update.EditedMessage!.Chat,
            UpdateType.ChannelPost => update.ChannelPost!.Chat,
            UpdateType.EditedChannelPost => update.EditedChannelPost!.Chat,
            UpdateType.ShippingQuery => null,
            UpdateType.PreCheckoutQuery => null,
            UpdateType.Poll => null,
            UpdateType.PollAnswer => null,
            UpdateType.MyChatMember => update.MyChatMember!.Chat,
            UpdateType.ChatMember => update.ChatMember!.Chat,
            UpdateType.ChatJoinRequest => update.ChatJoinRequest!.Chat,
            _ => null
        };
    }
}