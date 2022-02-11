using Fluegram.Keyboards.Abstractions.Builders;

namespace Fluegram.Keyboards.Builders;

public class KeyboardMarkupRowBuilderBase<TKeyboardMarkupButton> : IKeyboardMarkupRowBuilder<TKeyboardMarkupButton>
{
    private readonly List<TKeyboardMarkupButton> _buttons;

    public KeyboardMarkupRowBuilderBase()
    {
        _buttons = new List<TKeyboardMarkupButton>();
    }

    public IEnumerable<TKeyboardMarkupButton> Build()
    {
        return _buttons;
    }

    public IKeyboardMarkupRowBuilder<TKeyboardMarkupButton> Use(TKeyboardMarkupButton button)
    {
        _buttons.Add(button);

        return this;
    }
}