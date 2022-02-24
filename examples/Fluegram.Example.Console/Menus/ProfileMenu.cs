using Fluegram.Menus;

namespace Fluegram.Example.Console.Menus;

public class ProfileMenu : MenuBase
{
    public ProfileMenu() : base("profile", _ => "Profile Menu", _ => "Profile")
    {
    }
}