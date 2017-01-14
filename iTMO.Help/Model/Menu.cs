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
        The101,
        Settings
    }

    class MenuOpt
    {
        public Dictionary<MenuTypes, MenuItem> Options = new Dictionary<MenuTypes, MenuItem>()
        {
            { MenuTypes.JournalHub, new MenuItem(typeof(JournalHub)) },
            { MenuTypes.MessageHub, new MenuItem(typeof(MessageHub)) },
            { MenuTypes.ScheduleHub, new MenuItem(typeof(ScheduleHub)) },
            { MenuTypes.The101,     new MenuItem(typeof(The101)) },
            { MenuTypes.Settings,   new MenuItem(typeof(SettingsHub)) }
        };
    }

    class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type Page { get; set; }

        public MenuItem() { }
        public MenuItem(Type page) { this.Page = page; }
    }
}