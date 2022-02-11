using System.Reflection;
using Microsoft.CodeAnalysis;
using Telegram.Bot;

namespace Fluegram.SourceGenerator.Parameters;

public enum GeneratorMode
{
    Abstractions,
    Implementations
}

[Generator]
public class ParametersGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        // if (!Debugger.IsAttached)
        //     Debugger.Launch();
#endif
    }

    public void Execute(GeneratorExecutionContext context)
    {
        Type extensionsClassType = typeof(TelegramBotClientExtensions);

        GeneratorMode mode = context.Compilation.AssemblyName!.Contains("Abstractions")
            ? GeneratorMode.Abstractions
            : GeneratorMode.Implementations;

        bool abstractions = mode is GeneratorMode.Abstractions;

        string memberType = abstractions ? "interface" : "class";

        var methods = extensionsClassType.GetMethods(BindingFlags.Public | BindingFlags.Static);

        List<ParameterInfo> infos = new List<ParameterInfo>();

        foreach (MethodInfo methodInfo in extensionsClassType.GetMethods())
        {
            infos.AddRange(methodInfo.GetParameters());
        }

        var filtered = infos.DistinctBy(_ => $"{_.Name} {GetFullTypeName(_.ParameterType)}")
            .Where(_ => _.Name != "cancellationToken" && _.Name != "btClient")
            .Reverse().ToList();

        List<string> generated = new();

        Dictionary<(string Name, Type Type), string> hasNames = new();

        foreach (var parameter in filtered)
        {
            string parameterName = parameter.Name!.ToPascal();

            bool alreadyAdded = generated.Contains(parameterName);

            string parameterType = GetFullTypeName(parameter.ParameterType);

            string source = $@"

public interface IHas{(alreadyAdded ? "Different" : "")}{parameterName}
{{
    {parameterType} {parameterName} {{ get; set; }}
}}

public interface IHas{(alreadyAdded ? "Different" : "")}{parameterName}<T> : IHas{(alreadyAdded ? "Different" : "")}{parameterName}   
{{
    T Use{parameterName}({parameterType} {parameter.Name});
}}

";
            
            

            string sourceWithNamespace = $"namespace Fluegram.Parameters{(abstractions ? ".Abstractions.Has" : ".Has")};\n{source}";
            
            
            if(abstractions)
                context.AddSource($"IHas{(alreadyAdded ? "Different" : "")}{parameterName}.cs", sourceWithNamespace);

            hasNames[(parameter.Name!, parameter.ParameterType)] = $"IHas{(alreadyAdded ? "Different" : "")}{parameterName}";

            generated.Add(parameterName);
        }

        List<string> addedMethods = new();

        foreach (MethodInfo methodInfo in methods)
        {
            string name = methodInfo.Name.Replace("Async", "");

            
            bool exists = addedMethods.Contains(methodInfo.Name);

            if (exists)
            {
                
            }
            
            string existsString = exists ? "Extended" : "";
            
            string memberName = abstractions ? $"I{name}{existsString}Parameters" : $"{name}{existsString}Parameters";

            bool genericReturnType = methodInfo.ReturnType is { IsGenericType: true } returnType &&
                                     returnType.GetGenericTypeDefinition() == typeof(Task<>);


            var parameters = methodInfo.GetParameters().Skip(1).SkipLast(1).ToList();

            string hasNameDefinitions = string.Join(", ", parameters.Select(_ => "Fluegram.Parameters.Abstractions.Has." + hasNames[(_.Name!, _.ParameterType)] + $"<{memberName}>"));


            List<string> classDefinitions = new();


            string genericDefinition =
                (genericReturnType ? $"<{GetFullTypeName(methodInfo.ReturnType.GetGenericArguments().First())}>" : "");


            if (!abstractions)
            {
                List<string> parameterBindings = new();

                foreach (var parameter in parameters)
                {
                    string parameterName = parameter.Name!.ToPascal();

                    string parameterType = GetFullTypeName(parameter.ParameterType);

                    
                    string parameterMember = $@"

public {parameterType} {parameterName} {{ get; set; }}

public Fluegram.Parameters.Abstractions.I{memberName} Use{parameterName}({parameterType} {parameter.Name})
{{
    {parameterName} = {parameter.Name};    

    return this;
}}


";
                    
                    classDefinitions.Add(parameterMember);
                    
                    parameterBindings.Add($"{parameter.Name}: {parameter.Name!.ToPascal()}");
                }
  
                
                string invokeMethod = $@"

public async System.Threading.Tasks.Task{genericDefinition} InvokeAsync(Telegram.Bot.ITelegramBotClient telegramBotClient, CancellationToken cancellationToken = default)
{{
    {(genericReturnType ? "return" : "")} await Telegram.Bot.TelegramBotClientExtensions.{methodInfo.Name}(telegramBotClient, {string.Join(",", parameterBindings)}{(parameterBindings.Count > 0 ? ", " : "")}cancellationToken: cancellationToken);
}}

";

                classDefinitions.Add(invokeMethod);
            }


            string parametersClassSource = $@"
public {memberType} {memberName} : {(abstractions ? $"Fluegram.Parameters.Abstractions.IParameters{genericDefinition}{(hasNameDefinitions.Any() ? "," : "")} {hasNameDefinitions}" : $"Fluegram.Parameters.Abstractions.I{memberName}")}  
{{
    {string.Join("\n", classDefinitions)}
}}";
            
            addedMethods.Add(methodInfo.Name);

            string sourceWithNamespace = $"namespace Fluegram.Parameters{(abstractions ? ".Abstractions" : "")};\n{parametersClassSource}";
            
            context.AddSource($"{memberName}.cs", sourceWithNamespace);
        }
    }


    private string GetFullTypeName(Type type)
    {
        if (type.GetGenericArguments().Length == 0)
        {
            return type.Namespace + "." + type.Name;
        }

        var genericArguments = type.GetGenericArguments();
        var typeDefeninition = type.Namespace + "." + type.Name;
        var unmangledName = typeDefeninition.Substring(0, typeDefeninition.IndexOf("`", StringComparison.Ordinal));
        return unmangledName + "<" + String.Join(",", genericArguments.Select(GetFullTypeName)) + ">";
    }
}