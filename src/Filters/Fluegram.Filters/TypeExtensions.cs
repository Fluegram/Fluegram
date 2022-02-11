using System.Reflection;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Filters.Abstractions.Filtering;

namespace Fluegram.Filters;

public static class TypeExtensions
{
    public static IEnumerable<IFilter<TEntityContext, TEntity>> FindFiltersFor<TEntityContext, TEntity, T>()
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        foreach (var customAttribute in typeof(T).GetCustomAttributes())
        {
            if (customAttribute is IFilter<TEntityContext, TEntity> filter)
            {
                yield return filter;
            }
        }
    }
    
    public static IEnumerable<IFilter<TEntityContext, TEntity>> FindFiltersFor<TEntityContext, TEntity>(this MethodInfo methodInfo)
        where TEntityContext : IEntityContext<TEntity> where TEntity : class
    {
        foreach (var customAttribute in methodInfo.GetCustomAttributes())
        {
            if (customAttribute is IFilter<TEntityContext, TEntity> filter)
            {
                yield return filter;
            }
        }
    }
}