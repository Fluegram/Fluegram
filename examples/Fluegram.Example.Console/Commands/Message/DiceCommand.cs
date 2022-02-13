using System.Security.Cryptography;
using Fluegram.Commands;
using Fluegram.Parameters;
using Fluegram.Types.Contexts;
using Telegram.Bot.Types.Enums;

namespace Fluegram.Example.Console.Commands.Message;


public class DiceCommand : CommandBase<Telegram.Bot.Types.Message>
{
    private readonly Emoji[] _emojis;
    
    public DiceCommand() : base("dice")
    {
        _emojis = Enum.GetValues<Emoji>();
    }

    public override async Task ProcessAsync(EntityContext<Telegram.Bot.Types.Message> entityContext, CancellationToken cancellationToken)
    {
        Emoji emoji = _emojis[RandomNumberGenerator.GetInt32(0, _emojis.Length)];

        var diceMessage = await entityContext
            .InvokeAsync<SendDiceParameters, Telegram.Bot.Types.Message>(_ => _.UseEmoji(emoji),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        
        await entityContext
            .InvokeAsync<SendTextMessageParameters, Telegram.Bot.Types.Message>(_ => _.UseText($"Значение {emoji.ToString()}: {diceMessage.Dice!.Value}"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}