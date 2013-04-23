using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BadSnowstorm
{
    public class MenuInputProcessor : IInputProcessor
    {
        private readonly List<MenuItem> _menuItems;

        public MenuInputProcessor(List<MenuItem> menuItems)
        {
            _menuItems = menuItems;
        }

        public IAcceptsInput Process(IConsole console)
        {
            var menuSelection = GetMenuSelection(console);
            var selectedMenuItem = _menuItems.Single(menuItem => menuItem.Id == menuSelection);
            return selectedMenuItem;
        }

        private char GetMenuSelection(IConsole console)
        {
            char c;

            do
            {
                c = console.ReadKey();
            } while (_menuItems.All(menuItem => menuItem.Id != c));

            console.Write(c);
            Thread.Sleep(250);
            console.CursorLeft = console.CursorLeft - 1;
            console.Write(' ');

            return c;
        }
    }
}