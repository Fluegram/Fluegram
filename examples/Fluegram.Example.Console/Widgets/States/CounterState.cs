using Fluegram.Widgets.Abstractions;

namespace Fluegram.Example.Console.Widgets.States;

public class CounterState : IWidgetState<CounterState>
{
    public int Counter { get; set; }
}