using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Parameters.Abstractions;
using Fluegram.Parameters.Abstractions.Has;

namespace Fluegram.Parameters;

public static class EntityContextExtensions
{
    public static async Task InvokeAsync<TParameters>(this IContext entityContext, Action<TParameters> configure,
        CancellationToken cancellationToken = default)
        where TParameters : IParameters, new()
    {
        TParameters parameters = new();

        if (parameters is IHasChatId hasChatId) hasChatId.ChatId = entityContext.Chat!;

        configure(parameters);

        await parameters.InvokeAsync(entityContext.Client, cancellationToken);
    }

    public static async Task<T> InvokeAsync<TParameters, T>(this IContext entityContext, Action<TParameters> configure,
        CancellationToken cancellationToken = default)
        where TParameters : IParameters<T>, new()
    {
        TParameters parameters = new();

        if (parameters is IHasChatId hasChatId) hasChatId.ChatId = entityContext.Chat!;

        configure(parameters);

        return await parameters.InvokeAsync(entityContext.Client, cancellationToken);
    }
}