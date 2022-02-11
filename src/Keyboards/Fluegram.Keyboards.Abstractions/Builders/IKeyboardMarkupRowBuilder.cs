using Fluegram.Abstractions.Builders;

namespace Fluegram.Keyboards.Abstractions.Builders;

public interface IKeyboardMarkupRowBuilder<TKeyboardMarkupButton> : IBuilder<IEnumerable<TKeyboardMarkupButton>>
{
    IKeyboardMarkupRowBuilder<TKeyboardMarkupButton> Use(TKeyboardMarkupButton button);
}