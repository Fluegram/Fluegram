namespace Fluegram.Commands.Parsing;

public delegate bool TryParseDelegate<T>(string source, out T? result);