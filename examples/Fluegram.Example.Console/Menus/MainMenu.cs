using Fluegram.Menus;

namespace Fluegram.Example.Console.Menus;

public class MainMenu : MenuBase
{
    public MainMenu() : base("menu", _ => "Main Menu", _ => "Menu")
    {
    }
}