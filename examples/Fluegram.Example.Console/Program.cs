using Autofac;
using CommandLine;
using CommandLine.Text;
using Fluegram;
using Fluegram.Abstractions;
using Fluegram.Abstractions.Builders;
using Fluegram.Commands;
using Fluegram.Example.Console.Commands;
using Fluegram.Example.Console.Commands.Message;
using Fluegram.Example.Console.Commands.Message.Menus;
using Fluegram.Example.Console.Handlers.Message;
using Fluegram.Example.Console.Menus;
using Fluegram.Example.Console.Services;
using Fluegram.Example.Console.Widgets;
using Fluegram.Example.Console.Widgets.States;
using Fluegram.Handlers;
using Fluegram.Menus;
using Fluegram.Sessions;
using Fluegram.Types.Contexts;
using Fluegram.Widgets;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

await CommandLine.Parser.Default.ParseArguments<CommandLineArguments>(args)
    .WithParsedAsync(StartAsync);

async Task StartAsync(CommandLineArguments arguments)
{
    ContainerBuilder containerBuilder = new ContainerBuilder();

    ITelegramBotClient client = new TelegramBotClient(arguments.Token);

    containerBuilder.RegisterInstance(client).AsImplementedInterfaces();

    containerBuilder.RegisterType<BotInformationService>().SingleInstance();

    containerBuilder.RegisterFluegram(ConfigureFluegramBot);

    IContainer container = containerBuilder.Build();

    await container.Resolve<BotInformationService>().InitializeAsync();

    IFluegramBot bot = container.Resolve<IFluegramBot>();


    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    ReceiverOptions receiverOptions = new()
    {
        ThrowPendingUpdates = true,
    };

    await client.ReceiveAsync((_, update, token) => { bot.ProcessUpdateAsync(update, token).ConfigureAwait(false); },
        (_, exception, token) =>
        {
            //Handle exception here.
        },
        receiverOptions,
        cancellationTokenSource.Token);
}


void ConfigureFluegramBot(IFluegramBotBuilder fluegramBotBuilder)
{
    fluegramBotBuilder.UseFor<EntityContext<Message>, Message>(UpdateType.Message, ConfigureMessagesPipelineFeatures,
        ConfigureMessagesPipeline);

    fluegramBotBuilder.UseFor<EntityContext<CallbackQuery>, CallbackQuery>(UpdateType.CallbackQuery,
        ConfigureCallbackQueriesPipelineFeatures,
        ConfigureCallbackQueriesPipeline);

    fluegramBotBuilder.UseCommandParsing(_ => _
        .UseEntityTextManipulator<Message>(_ => _.Text ?? _.Caption ?? "", (entity, text) =>
        {
            if (entity.Text is { Length: > 0 })
            {
                entity.Text = text;
            }
            else
            {
                entity.Caption = text;
            }
        }).UseCommandNameRetriever((ctx, id) => id));
}

void ConfigureMessagesPipelineFeatures(
    IPipelineFeaturesConfigurator<EntityContext<Message>, Message> featuresConfigurator)
{
    featuresConfigurator.UseSessionManagement();

    featuresConfigurator.UseWidgets(configurator => configurator
        .Use<CounterWidget, CounterState>());
}

void ConfigureMessagesPipeline(IPipelineBuilder<EntityContext<Message>, Message> pipelineBuilder)
{
    pipelineBuilder
        .UseSession()
        .UseHandlers(_ => _
            .Use<TextNormalizerHandler>())
        .UseCommands(_ => _
            .Use<MainMenuCommand>(_ => _
                .Use<ProfileMenuCommand>(_ => _
                    .Use<EditProfileMenuCommand>(_ => _
                        .Use<EditProfileNameMenuCommand>()
                    )
                )
            )
            .Use<TypingCommand>()
            .Use<CounterCommand>()
            .Use<DiceCommand>()
            .Use<RandomCommand, RandomArguments>());
}


void ConfigureCallbackQueriesPipelineFeatures(
    IPipelineFeaturesConfigurator<EntityContext<CallbackQuery>, CallbackQuery> featuresConfigurator)
{
    featuresConfigurator.UseSessionManagement();

    featuresConfigurator.UseMenus(_ => _
        .Use<MainMenu>(_ => _
            .Use<ProfileMenu>(_ => _
                .Use<EditProfileMenu>(_ => _
                    .Use<EditProfileNameMenu>()))));
}

void ConfigureCallbackQueriesPipeline(IPipelineBuilder<EntityContext<CallbackQuery>, CallbackQuery> pipelineBuilder)
{
    pipelineBuilder.UseHandlers(_ => _
        .Use<MenuHandler<EntityContext<CallbackQuery>, MainMenu>>());
}


public class CommandLineArguments
{
    [CommandLine.Option('t', "token", HelpText = "Telegram Bot API Token")]
    public string Token { get; set; }
}