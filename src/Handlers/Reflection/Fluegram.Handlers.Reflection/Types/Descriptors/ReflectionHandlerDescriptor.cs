using System.Reflection;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Fluegram.Filters.Abstractions.Filtering;
using Fluegram.Handlers.Reflection.Abstractions.Types.Descriptors;
using TypeExtensions = Fluegram.Filters.TypeExtensions;

namespace Fluegram.Handlers.Reflection.Types.Descriptors;

public struct
    ReflectionHandlerDescriptor<TEntityContext, TEntity> : IReflectionHandlerDescriptor<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    public static ReflectionHandlerDescriptor<TEntityContext, TEntity> CreateFromMethodInfo(MethodInfo methodInfo)
    {
        var filters = TypeExtensions.FindFiltersFor<TEntityContext, TEntity>(methodInfo).ToArray();

        var preProcessingActions = Actions.TypeExtensions
            .FindPreProcessingActionsFor<TEntityContext, TEntity>(methodInfo).ToArray();
        var postProcessingActions = Actions.TypeExtensions
            .FindPostProcessingActionsFor<TEntityContext, TEntity>(methodInfo).ToArray();


        return new ReflectionHandlerDescriptor<TEntityContext, TEntity>(methodInfo.DeclaringType, methodInfo,
            filters, preProcessingActions, postProcessingActions, null);
    }

    private ReflectionHandlerDescriptor(Type? type, MethodInfo method,
        IEnumerable<IFilter<TEntityContext, TEntity>> filters,
        IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> preProcessingActions,
        IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> postProcessingActions, string? key)
    {
        Type = type;
        Filters = filters;
        Method = method;
        Name = key;
        PreProcessingActions = preProcessingActions;
        PostProcessingActions = postProcessingActions;
    }

    public Type? Type { get; }
    public string? Name { get; }
    public IEnumerable<IFilter<TEntityContext, TEntity>> Filters { get; }
    public IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> PreProcessingActions { get; }
    public IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> PostProcessingActions { get; }
    public MethodInfo Method { get; }
}