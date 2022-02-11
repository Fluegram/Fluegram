using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public class FuncEntityTextManipulator<TEntity> : IEntityTextManipulator<TEntity> where TEntity : class
{
    private readonly Func<TEntity, string> _getFunc;
    private readonly Action<TEntity, string> _setFunc;

    public FuncEntityTextManipulator(Func<TEntity, string> getFunc, Action<TEntity, string> setFunc)
    {
        _getFunc = getFunc;
        _setFunc = setFunc;
    }

    public string Get(TEntity entity) => _getFunc(entity);

    public void Set(TEntity entity, string value) => _setFunc(entity, value);
}
