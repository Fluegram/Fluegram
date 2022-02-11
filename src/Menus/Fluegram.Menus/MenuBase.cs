using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Menus.Abstractions;

namespace Fluegram.Menus;

public class MenuBase : List<IMenu>, IMenu
{
    public MenuBase(string id, Func<IContext, string> text, Func<IContext, string> name)
    {
        Id = id;
        Text = text;
        Name = name;
    }

    public string Id { get; }
    
    public IMenu? Parent { get; set; }
    
    public Func<IContext, string> Name { get; }
    public Func<IContext, string> Text { get; }
}