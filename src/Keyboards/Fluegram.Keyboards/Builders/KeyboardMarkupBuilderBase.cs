using Fluegram.Abstractions.Builders;
using Fluegram.Keyboards.Abstractions.Builders;

namespace Fluegram.Keyboards.Builders;

public abstract class KeyboardMarkupBuilderBase<TKeyboardMarkup, TKeyboardMarkupRowBuilder, TKeyboardMarkupButton> 
    : IKeyboardMarkupBuilder<TKeyboardMarkup, TKeyboardMarkupRowBuilder, TKeyboardMarkupButton> 
    where TKeyboardMarkupRowBuilder : IBuilder<List<TKeyboardMarkupButton>>
{
    private readonly List<List<TKeyboardMarkupButton>> _buttons;

    public KeyboardMarkupBuilderBase()
    {
        _buttons = new List<List<TKeyboardMarkupButton>>();
    }
    
    public TKeyboardMarkup Build()
    {
        return Build(_buttons);
    }

    public IKeyboardMarkupBuilder<TKeyboardMarkup, TKeyboardMarkupRowBuilder, TKeyboardMarkupButton> UseRow(Action<TKeyboardMarkupRowBuilder> configureRow)
    {
        TKeyboardMarkupRowBuilder rowBuilder = CreateRowBuilder();

        configureRow(rowBuilder);
        
        _buttons.Add(rowBuilder.Build().ToList());

        return this;
    }
    
    protected abstract TKeyboardMarkup Build(IEnumerable<IEnumerable<TKeyboardMarkupButton>> buttons);
    
    protected abstract TKeyboardMarkupRowBuilder CreateRowBuilder();
}