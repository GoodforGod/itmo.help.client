using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTMO.Help.View;

namespace iTMO.Help.Model
{
    enum MenuTypes
    {
        JournalHub,
        MessageHub,
        ScheduleHub,
        Settings
    }

    class Menu
    {
        public Dictionary<MenuTypes, MenuItem> Menus;

        public Menu()
        {
            Menus = new Dictionary<MenuTypes, MenuItem>()
            {
                { MenuTypes.JournalHub, new MenuItem(typeof(JournalHub)) },
                { MenuTypes.MessageHub, new MenuItem(typeof(MessageHub)) },
                { MenuTypes.ScheduleHub, new MenuItem(typeof(ScheduleHub)) },
                { MenuTypes.Settings, new MenuItem(typeof(SettingsHub)) }
            };
        }
    }

    class MenuItem
    {
        public Type Page { get; set; }
        public MenuItem(Type page) { this.Page = page; }
    }
}
