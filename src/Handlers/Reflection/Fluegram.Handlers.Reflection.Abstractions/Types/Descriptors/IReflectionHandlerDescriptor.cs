using System.Reflection;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Abstractions.Types.Descriptors;
using Fluegram.Actions.Abstractions;
using Fluegram.Filters.Abstractions.Filtering;

namespace Fluegram.Handlers.Reflection.Abstractions.Types.Descriptors;

public interface IReflectionHandlerDescriptor<TEntityContext, TEntity> : ITypedDescriptor
    where TEntityContext : IEntityContext<TEntity> where TEntity : class
{
    IEnumerable<IFilter<TEntityContext, TEntity>> Filters { get; }

    IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> PreProcessingActions { get; }

    IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> PostProcessingActions { get; }

    MethodInfo Method { get; }
}