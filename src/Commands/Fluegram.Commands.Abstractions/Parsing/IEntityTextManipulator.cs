namespace Fluegram.Commands.Abstractions.Parsing;

public interface IEntityTextManipulator<TEntity>
    where TEntity : class
{
    string Get(TEntity entity);

    void Set(TEntity entity, string value);
}