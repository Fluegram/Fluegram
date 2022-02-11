using Fluegram.Abstractions.Builders;

namespace Fluegram.Keyboards.Abstractions.Builders;

public interface IKeyboardMarkupBuilder<TKeyboardMarkup, TKeyboardMarkupRowBuilder, TKeyboardMarkupButton> : IBuilder<TKeyboardMarkup>
    where TKeyboardMarkupRowBuilder : IKeyboardMarkupRowBuilder<TKeyboardMarkupButton>
{
    IKeyboardMarkupBuilder<TKeyboardMarkup, TKeyboardMarkupRowBuilder, TKeyboardMarkupButton> UseRow(Action<TKeyboardMarkupRowBuilder> configureRow);
}