using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public class FuncCommandNameRetriever : ICommandNameRetriever
{
    private readonly Func<IContext, string, string> _retrieverFunc;

    public FuncCommandNameRetriever(Func<IContext, string, string> retrieverFunc)
    {
        _retrieverFunc = retrieverFunc;
    }
    
    public string Retrieve(IContext entityContext, string commandId) => _retrieverFunc(entityContext, commandId);
}