using Fluegram.Example.Console.Menus;

namespace Fluegram.Example.Console.Commands.Message.Menus;

public class ProfileMenuCommand : SendMenuCommandBase<ProfileMenu>
{
    public ProfileMenuCommand() : base("profile")
    {
    }
}