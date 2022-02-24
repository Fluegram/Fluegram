using System.Reflection;
using Autofac;
using Fluegram.Commands.Abstractions.Parsing;

namespace Fluegram.Commands.Parsing;

public class CommandArgumentsParser<TArguments> : ICommandArgumentsParser<TArguments> where TArguments : class, new()
{
    private readonly PropertyInfo[] _properties;
    private readonly IComponentContext _components;

    public CommandArgumentsParser(PropertyInfo[] properties, IComponentContext components)
    {
        _properties = properties;
        _components = components;
    }

    public ICommandArgumentsParseResult<TArguments> Parse(string sourceText)
    {
        TArguments arguments = new TArguments();

        TextSegmentCollection segments = _components.Resolve<TextSegmentCollection>(new PositionalParameter(0, sourceText));

        ICommandArgumentParseError[] errors = Array.Empty<ICommandArgumentParseError>();

        for (int index = 0; index < _properties.Length; index++)
        {
            PropertyInfo propertyInfo = _properties[index];

            if (!segments.SegmentsAvailable)
            {
                AddError(new CommandArgumentParseError(new CommandArgument(propertyInfo.Name), null));

                break;
            }
            
            string segment = segments.Take();

            bool argumentSet = false;
            
            if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(arguments, segment);
                argumentSet = true;
            }
            else if (Type.GetTypeCode(propertyInfo.PropertyType) is { } typeCode && typeCode is not TypeCode.Object or TypeCode.Empty or TypeCode.DBNull)
            {
                try
                {
                    propertyInfo.SetValue(arguments, Convert.ChangeType(segment, typeCode));
                    argumentSet = true;
                }
                catch (Exception exception)
                {
                    AddError(new CommandArgumentParseError(new CommandArgument(propertyInfo.Name), exception));
                }
            }

            if (!argumentSet)
            {
                var resolver = _components.ResolveOptional(typeof(ICommandArgumentTypeResolver<>).MakeGenericType(propertyInfo.PropertyType));

                if (resolver is { })
                {
                    try
                    {
                        object? argumentValue = resolver.GetType().GetMethod(nameof(ICommandArgumentTypeResolver<object>.Resolve))!.Invoke(resolver, new[] { segment });

                        if (argumentValue is { })
                        {
                            propertyInfo.SetValue(arguments, argumentValue);
                        }
                        else
                        {
                            AddError(new CommandArgumentParseError(new CommandArgument(propertyInfo.Name), null));
                        }

                    }
                    catch (Exception exception)
                    {
                        AddError(new CommandArgumentParseError(new CommandArgument(propertyInfo.Name), exception));
                    }
                }
                else
                {
                    AddError(new CommandArgumentParseError(new CommandArgument(propertyInfo.Name), null));
                }
            }
        } 

        if (errors.Length > 0)
        {
            return new CommandArgumentsFailedParseResult<TArguments>(false, errors);
        }

        return new CommandArgumentsSuccessfulParseResult<TArguments>(true, arguments, segments.ToString());
        
        
        
        void AddError(ICommandArgumentParseError error)
        {
            Array.Resize(ref errors, errors.Length + 1);

            errors[^1] = error;
        }
    }
}