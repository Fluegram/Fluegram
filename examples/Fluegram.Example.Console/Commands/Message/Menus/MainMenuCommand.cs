using Fluegram.Example.Console.Menus;

namespace Fluegram.Example.Console.Commands.Message.Menus;

public class MainMenuCommand : SendMenuCommandBase<MainMenu>
{
    public MainMenuCommand() : base("menu")
    {
    }
}

public class ProfileMenuCommand : SendMenuCommandBase<ProfileMenu>
{
    public ProfileMenuCommand() : base("profile")
    {
    }
}

public class EditProfileMenuCommand : SendMenuCommandBase<EditProfileMenu>
{
    public EditProfileMenuCommand() : base("edit")
    {
    }
}

public class EditProfileNameMenuCommand : SendMenuCommandBase<EditProfileNameMenu>
{
    public EditProfileNameMenuCommand() : base("name")
    {
    }
}