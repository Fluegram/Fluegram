using Fluegram.Example.Console.Menus;

namespace Fluegram.Example.Console.Commands.Message.Menus;

public class MainMenuCommand : SendMenuCommandBase<MainMenu>
{
    public MainMenuCommand() : base("menu")
    {
    }
}