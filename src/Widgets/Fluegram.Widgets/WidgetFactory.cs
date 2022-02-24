using Autofac;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Widgets.Abstractions;

namespace Fluegram.Widgets;

public class
    WidgetFactory<TWidget, TEntityContext, TEntity, TState> : IWidgetFactory<TWidget, TEntityContext, TEntity, TState>
    where TWidget : IWidget<TEntityContext, TEntity, TState>
    where TEntityContext : IEntityContext<TEntity>
    where TEntity : class
    where TState : class, IWidgetState<TState>, new()
{
    public async Task<IWidgetController<TEntityContext, TEntity, TState>> CreateAsync(TEntityContext entityContext,
        CancellationToken cancellationToken)
    {
        var widget = entityContext.Components.Resolve<TWidget>();

        await widget.OnInitializedAsync(entityContext, cancellationToken).ConfigureAwait(false);

        var controller = new WidgetController<TEntityContext, TEntity, TState>(entityContext, widget);

        return controller;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}