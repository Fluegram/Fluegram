using Autofac;
using Fluegram.Abstractions.Builders;
using Fluegram.Abstractions.Types.Contexts;
using Fluegram.Menus.Abstractions;
using Telegram.Bot.Types;

namespace Fluegram.Menus;

public static class PipelineFeaturesConfiguratorExtensions
{
    public static IPipelineFeaturesConfigurator<TEntityContext, CallbackQuery> UseMenus<TEntityContext>(
        this IPipelineFeaturesConfigurator<TEntityContext, CallbackQuery> featuresConfigurator,
        Action<MenusConfigurator<TEntityContext>> configureMenus)
        where TEntityContext : IEntityContext<CallbackQuery>
    {
        var configurator =
            new MenusConfigurator<TEntityContext>(featuresConfigurator.Components);

        configureMenus(configurator);

        return featuresConfigurator;
    }

    public class MenusConfigurator<TEntityContext>
        where TEntityContext : IEntityContext<CallbackQuery>
    {
        private readonly ContainerBuilder _containerBuilder;

        public MenusConfigurator(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public MenusConfigurator<TEntityContext> Use<TMenu>(Action<MenuConfigurator<TMenu>> configureMenu)
            where TMenu : class, IMenu, new()
        {
            var menu = new TMenu();


            var menuConfigurator = new MenuConfigurator<TMenu>(menu, _containerBuilder);

            configureMenu(menuConfigurator);

            _containerBuilder.RegisterInstance(menu);

            return this;
        }

        public class MenuConfigurator<TMenu>
            where TMenu : class, IMenu, new()
        {
            private readonly ContainerBuilder _containerBuilder;

            private readonly TMenu _menu;

            public MenuConfigurator(TMenu menu, ContainerBuilder containerBuilder)
            {
                _menu = menu;
                _containerBuilder = containerBuilder;
            }

            public MenuConfigurator<TMenu> Use<TSubMenu>()
                where TSubMenu : class, IMenu, new()
            {
                var subMenu = new TSubMenu();

                subMenu.Parent = _menu;

                _menu.Add(subMenu);

                _containerBuilder.RegisterInstance(subMenu);

                return this;
            }

            public MenuConfigurator<TMenu> Use<TSubMenu>(Action<MenuConfigurator<TSubMenu>> configureSubMenu)
                where TSubMenu : class, IMenu, new()
            {
                var subMenu = new TSubMenu();

                subMenu.Parent = _menu;

                var subMenuConfigurator = new MenuConfigurator<TSubMenu>(subMenu, _containerBuilder);

                configureSubMenu(subMenuConfigurator);

                _containerBuilder.RegisterInstance(subMenu);

                _menu.Add(subMenu);

                return this;
            }
        }
    }
}