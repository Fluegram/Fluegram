using System.Reflection;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;

namespace Fluegram.Actions;

public static class TypeExtensions
{
    public static IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> FindPreProcessingActionsFor<TEntityContext, TEntity, T>()
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        foreach (var customAttribute in typeof(T).GetCustomAttributes())
        {
            if (customAttribute is IPreProcessingAction<TEntityContext, TEntity> action)
            {
                yield return action;
            }
        }
    }
    
    public static IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> FindPostProcessingActionsFor<TEntityContext, TEntity, T>()
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        foreach (var customAttribute in typeof(T).GetCustomAttributes())
        {
            if (customAttribute is IPostProcessingAction<TEntityContext, TEntity> action)
            {
                yield return action;
            }
        }
    }
    
    
    
    public static IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> FindPreProcessingActionsFor<TEntityContext, TEntity>(this MethodInfo methodInfo)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        foreach (var customAttribute in methodInfo.GetCustomAttributes())
        {
            if (customAttribute is IPreProcessingAction<TEntityContext, TEntity> action)
            {
                yield return action;
            }
        }
    }
    
    public static IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> FindPostProcessingActionsFor<TEntityContext, TEntity>(this MethodInfo methodInfo)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        foreach (var customAttribute in methodInfo.GetCustomAttributes())
        {
            if (customAttribute is IPostProcessingAction<TEntityContext, TEntity> action)
            {
                yield return action;
            }
        }
    }
}