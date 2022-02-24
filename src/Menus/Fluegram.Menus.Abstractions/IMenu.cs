using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Menus.Abstractions;

public interface IMenu : IList<IMenu>
{
    string Id { get; }

    IMenu? Parent { get; set; }

    Func<IContext, string> Name { get; }

    Func<IContext, string> Text { get; }
}