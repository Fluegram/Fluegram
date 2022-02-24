using Fluegram.Example.Console.Menus;

namespace Fluegram.Example.Console.Commands.Message.Menus;

public class EditProfileNameMenuCommand : SendMenuCommandBase<EditProfileNameMenu>
{
    public EditProfileNameMenuCommand() : base("name")
    {
    }
}