using Fluegram.Abstractions.Types.Contexts;

namespace Fluegram.Commands.Abstractions.Parsing;

public interface ICommandNameRetriever
{
    string Retrieve(IContext entityContext, string commandId);
}