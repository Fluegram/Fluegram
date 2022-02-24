namespace Fluegram.Widgets.Abstractions;

public interface IWidgetState<TState> where TState : IWidgetState<TState>
{
}