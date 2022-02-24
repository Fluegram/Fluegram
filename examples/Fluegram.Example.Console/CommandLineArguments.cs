using CommandLine;

public class CommandLineArguments
{
    [Option('t', "token", HelpText = "Telegram Bot API Token")]
    public string Token { get; set; }
}