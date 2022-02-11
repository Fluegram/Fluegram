using Fluegram.Abstractions.Middlewares;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Actions.Abstractions;
using Fluegram.Filters.Abstractions.Filtering;

namespace Fluegram.Handlers.Abstractions.Middlewares;

public interface IHandlerMiddleware<THandler, TEntityContext, TEntity> : IMiddleware<TEntityContext, TEntity>
    where THandler : IHandler<TEntityContext, TEntity>
    where TEntityContext : IEntityContext<TEntity> 
    where TEntity : class
{
    IEnumerable<IFilter<TEntityContext, TEntity>> Filters { get; }
    
    IEnumerable<IPreProcessingAction<TEntityContext, TEntity>> PreProcessingActions { get; }
    
    IEnumerable<IPostProcessingAction<TEntityContext, TEntity>> PostProcessingActions { get; }
}