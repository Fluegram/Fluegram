using System.Reflection;
using Autofac;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Commands.Attributes;

namespace Fluegram.Commands.Parsing;

public class CommandArgumentsParser : ICommandArgumentsParser
{
    private readonly bool _useQuote;
    private readonly bool _useDoubleQuote;
    private readonly StringComparison _stringComparison;
    private readonly IComponentContext _components;

    private readonly string _splitRegex;

    public CommandArgumentsParser(bool useQuote, bool useDoubleQuote, StringComparison stringComparison, IComponentContext components)
    {
        _useQuote = useQuote;
        _useDoubleQuote = useDoubleQuote;
        _stringComparison = stringComparison;
        _components = components;

        _splitRegex = BuildSplitRegex();
    }
    
    private string BuildSplitRegex()
    {
        string quotePart = _useQuote ? "'" : "",
            doubleQuotePart = _useDoubleQuote ? "\"" : "";

        var regex = $"[^\\s{doubleQuotePart}{quotePart}]+";

        if (_useDoubleQuote) regex += "|\"([^\"]+)\"";

        if (_useQuote) regex += "|\'([^\']+)\'";

        return regex;
    }
    
    
    public ICommandArgumentsParseResult<TArguments> Parse<TArguments>(string sourceText) where TArguments : class, new()
    {
        TArguments arguments = new TArguments();

        var properties = arguments.GetType().GetProperties();

        List<ICommandArgumentParseError> errors = new List<ICommandArgumentParseError>(properties.Length);

        TextSegmentCollection segments = new TextSegmentCollection(sourceText, _splitRegex, _useQuote, _useDoubleQuote);

        int index = 1;

        foreach (var property in properties)
        {
            if (!segments.SegmentsAvailable)
            {
                if(Nullable.GetUnderlyingType(property.PropertyType) is null)
                    errors.Add(new CommandArgumentParseError(new CommandArgument(property.Name), null));
                
                break;
            }
            
            if (property.GetCustomAttribute<CommandArgumentAttribute>() is { })
            {
                if (property.GetSetMethod() is null)
                {
                    throw new InvalidOperationException("Argument property should have public set accessor");
                }

                string segment = segments.Take();

                if (index == properties.Length && property.PropertyType == typeof(string))
                {
                    segment = segments.ToString();

                    segments = null!;
                }
                
                Type argumentType = property.PropertyType;

                object? argumentValue = null;

                try
                {
                    if (Nullable.GetUnderlyingType(property.PropertyType) is { } underlyingType)
                    {
                        argumentValue = Convert.ChangeType(segment, underlyingType);
                    }

                    else argumentValue = Convert.ChangeType(segment, argumentType);
                }
                catch (Exception exception)
                {
                    if(Nullable.GetUnderlyingType(property.PropertyType) is null)
                        errors.Add(new CommandArgumentParseError(new CommandArgument(property.Name), exception));
                }

                if (argumentValue is null && _components.ResolveOptional(typeof(ICommandArgumentTypeResolver<>).MakeGenericType(argumentType)) is { } resolver)
                {
                    var resolveMethod = resolver.GetType().GetMethod("Resolve")!;

                    try
                    {
                        argumentValue = resolveMethod.Invoke(resolver, new[]
                        {
                            segment
                        });
                    }
                    catch (Exception exception)
                    {
                        errors.Add(new CommandArgumentParseError(new CommandArgument(property.Name), exception));
                    }
                }
                
                if(argumentValue is null && !IsNullable(argumentType) && errors[^1].Argument.Name != property.Name)
                    errors.Add(new CommandArgumentParseError(new CommandArgument(property.Name), null));
                
                if(argumentValue is not null)
                    property.SetValue(arguments, argumentValue);

                index++;
            }
        }

        if (errors.Count > 0)
        {
            return new CommandArgumentsFailedParseResult<TArguments>(false, errors);
        }
        
        return new CommandArgumentsSuccessfulParseResult<TArguments>(true, arguments, segments.ToString());
    }
    
    private static bool IsNullable(Type type)
    {
        if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
        return false; // value-type
    }
}