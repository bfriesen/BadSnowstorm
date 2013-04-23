using System.Collections.Generic;
using System;
using System.Linq;

namespace BadSnowstorm
{
    public class Menu : Input
    {
        public Menu()
            : base(InputType.Menu)
        {
            MenuItems = new List<MenuItem>();
            Prompt = "Select a menu item: ";
        }

        public List<MenuItem> MenuItems { get; private set; }

        public override string GetContent()
        {
            return string.Join(
                Environment.NewLine,
                MenuItems.Select(menuItem => string.Format("{0}) {1}", menuItem.Id, menuItem.Text))
                         .Concat(new[] { " ", Prompt })
                         .ToArray());
        }
    }
}