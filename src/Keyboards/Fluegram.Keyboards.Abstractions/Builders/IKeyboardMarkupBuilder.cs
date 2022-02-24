using Fluegram.Abstractions.Builders;

namespace Fluegram.Keyboards.Abstractions.Builders;

public interface
    IKeyboardMarkupBuilder<TKeyboardMarkup, TKeyboardMarkupRowBuilder,
        TKeyboardMarkupButton> : IBuilder<TKeyboardMarkup>
{
    IKeyboardMarkupBuilder<TKeyboardMarkup, TKeyboardMarkupRowBuilder, TKeyboardMarkupButton> UseRow(
        Action<TKeyboardMarkupRowBuilder> configureRow);
}