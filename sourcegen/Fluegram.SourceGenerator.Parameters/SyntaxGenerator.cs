/*using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Fluegram.SourceGenerator.Parameters.GeneratorMode;

namespace Fluegram.SourceGenerator.Parameters;

public enum GeneratorMode
{
    Abstractions,
    Implementations
}

public class SyntaxGenerator
{
    private readonly GeneratorMode _mode;

    public SyntaxGenerator(GeneratorMode mode)
    {
        _mode = mode;
    }

    public TypeDeclarationSyntax GenerateType(MethodInfo info)
    {
        

        string source = $@"
;

    }

    public TypeDeclarationSyntax GenerateHas(ParameterInfo parameter)
    {
        string parameterName = parameter.Name.ToPascal();

        string hasName = $"Has{parameterName}";

        TypeDeclarationSyntax type = _mode is Abstractions
            ? InterfaceDeclaration($"I{hasName}")
            : ClassDeclaration(hasName);

        type = (TypeDeclarationSyntax)type
            .AddBaseListTypes(SimpleBaseType(ParseTypeName(
                "")));
    }


    private string GetFullTypeName(Type t)
    {
        if (t.IsArray)
        {
            return GetFullTypeName(t.GetElementType()) + "[]";
        }

        if (t.IsGenericType)
        {
            return string.Format(
                "{0}.{1}<{1}>",
                t.Namespace,
                t.Name.Substring(0, t.Name.LastIndexOf("`", StringComparison.InvariantCulture)),
                string.Join(", ", t.GetGenericArguments().Select(GetFullTypeName)));
        }

        return t.Name;
    }

    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}*/

