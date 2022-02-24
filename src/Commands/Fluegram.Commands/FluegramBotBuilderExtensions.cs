using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Commands.Abstractions.Parsing;
using Fluegram.Commands.Parsing;

namespace Fluegram.Commands;

public static class FluegramBotBuilderExtensions
{
    public static IFluegramBotBuilder UseCommandParsing(this IFluegramBotBuilder fluegramBot,
        Action<CommandParsingConfigurator> configure)
    {
        var configurator = new CommandParsingConfigurator(fluegramBot.Components);

        configure(configurator);

        string quotePart = configurator.ShouldUseQuote ? "'" : "",
            doubleQuotePart = configurator.ShouldUseDoubleQuote ? "\"" : "";

        var regex = $"[^\\s{doubleQuotePart}{quotePart}]+";

        if (configurator.ShouldUseDoubleQuote) regex += "|\"([^\"]+)\"";

        if (configurator.ShouldUseQuote) regex += "|\'([^\']+)\'";
        
        fluegramBot.Components.RegisterType<TextSegmentCollection>().AsSelf()
            .WithParameter(new PositionalParameter(1, regex))
            .WithParameter(new PositionalParameter(2, configurator.ShouldUseQuote))
            .WithParameter(new PositionalParameter(3, configurator.ShouldUseDoubleQuote));


        return fluegramBot;
    }

    public class CommandParsingConfigurator
    {
        private readonly ContainerBuilder _containerBuilder;
        internal bool ShouldUseDoubleQuote;
        internal bool ShouldUseQuote;
        internal StringComparison StringComparison;

        public CommandParsingConfigurator(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public CommandParsingConfigurator UseQuote(bool useQuote = false)
        {
            ShouldUseQuote = useQuote;

            return this;
        }

        public CommandParsingConfigurator UseDoubleQuote(bool useDoubleQuote = false)
        {
            ShouldUseDoubleQuote = useDoubleQuote;

            return this;
        }

        public CommandParsingConfigurator UseStringComparison(StringComparison stringComparison)
        {
            StringComparison = stringComparison;

            return this;
        }

        public CommandParsingConfigurator UseArgumentTypeResolver<T>(Func<string, T> resolverFunc)
        {
            _containerBuilder.RegisterType<FuncCommandArgumentTypeResolver<T>>()
                .UsingConstructor(resolverFunc.GetType())
                .WithParameter(new PositionalParameter(0, resolverFunc))
                .AsImplementedInterfaces();

            return this;
        }

        public CommandParsingConfigurator UseArgumentTypeResolver<T>(TryParseDelegate<T> resolverDelegate)
        {
            _containerBuilder.RegisterType<FuncCommandArgumentTypeResolver<T>>()
                .UsingConstructor(resolverDelegate.GetType())
                .WithParameter(new PositionalParameter(0, resolverDelegate));

            return this;
        }

        public CommandParsingConfigurator UseArgumentTypeResolver<TArgumentTypeResolver, T>()
            where TArgumentTypeResolver : ICommandArgumentTypeResolver<T>
        {
            _containerBuilder.RegisterType<TArgumentTypeResolver>();

            return this;
        }

        public CommandParsingConfigurator UseCommandNameRetriever<TCommandNameRetriever>()
            where TCommandNameRetriever : ICommandNameRetriever
        {
            _containerBuilder.RegisterType<TCommandNameRetriever>().AsImplementedInterfaces();

            return this;
        }

        public CommandParsingConfigurator UseCommandNameRetriever(Func<IContext, string, string> func)
        {
            _containerBuilder.RegisterType<FuncCommandNameRetriever>().WithParameter(new PositionalParameter(0, func))
                .AsImplementedInterfaces();

            return this;
        }

        public CommandParsingConfigurator UseEntityTextManipulator<TEntity>(Func<TEntity, string> getFunc,
            Action<TEntity, string> setFunc) where TEntity : class
        {
            _containerBuilder.RegisterType<FuncEntityTextManipulator<TEntity>>()
                .WithParameter(new PositionalParameter(0, getFunc))
                .WithParameter(new PositionalParameter(1, setFunc))
                .AsImplementedInterfaces();

            return this;
        }
    }
}