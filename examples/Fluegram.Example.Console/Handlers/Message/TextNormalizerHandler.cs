using System.Text.RegularExpressions;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Example.Console.Services;
using Fluegram.Handlers;
using Fluegram.Types.Contexts;

namespace Fluegram.Example.Console.Handlers.Message;

public class TextNormalizerHandler : HandlerBase<Telegram.Bot.Types.Message>
{
    private readonly Regex _appealRegex;
    private readonly BotInformationService _botInformationService;
    private readonly IEntityTextManipulator<Telegram.Bot.Types.Message> _entityTextManipulator;

    public TextNormalizerHandler(IEntityTextManipulator<Telegram.Bot.Types.Message> entityTextManipulator,
        BotInformationService botInformationService)
    {
        _entityTextManipulator = entityTextManipulator;
        _botInformationService = botInformationService;
        _appealRegex = new Regex($"/(?<CommandId>\\w+)(@{_botInformationService.Bot.Username})?");
    }

    public override async Task HandleAsync(EntityContext<Telegram.Bot.Types.Message> entityContext,
        CancellationToken cancellationToken)
    {
        var text = _entityTextManipulator.Get(entityContext.Entity);

        var resultText = text;

        if (_appealRegex.Match(text) is { Success: true, Groups: { } groups }) resultText = groups["CommandId"].Value;

        _entityTextManipulator.Set(entityContext.Entity, resultText);
    }
}