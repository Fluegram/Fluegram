using Fluegram.Example.Console.Menus;

namespace Fluegram.Example.Console.Commands.Message.Menus;

public class EditProfileMenuCommand : SendMenuCommandBase<EditProfileMenu>
{
    public EditProfileMenuCommand() : base("edit")
    {
    }
}