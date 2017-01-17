﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using iTMO.Help.Model;
using iTMO.Help.View;

namespace iTMO.Help
{
    public sealed partial class MainPage : Page
    {
        List<MenuItem> MenuOptions = new List<MenuItem>()
        {
            new MenuItem() { Page = typeof(JournalHub),     Title = "Journal",  Icon = "\xE2AC" },
            new MenuItem() { Page = typeof(ScheduleHub),    Title = "Schedule", Icon = "\xE163" },
            new MenuItem() { Page = typeof(ExamHub),        Title = "Exams",    Icon = "\xE184" },
            new MenuItem() { Page = typeof(MessageHub),     Title = "Messages", Icon = "\xE119" },
            new MenuItem() { Page = typeof(The101),         Title = "Room 101", Icon = "\xE70C" },
            new MenuItem() { Page = typeof(SettingsHub),    Title = "Settings", Icon = "\xE115" }
        };
    }

    public class MenuItem
    {
        public string   Title   { get; set; } = "";
        public string   Icon    { get; set; } = "";
        public Type     Page    { get; set; }
    }

    public class CheckResponse
    {
        public bool     IsValid   { get; set; } = false;
        public string   Login     { get; set; }
        public string   Password  { get; set; }
        public string   Message   { get; set; } = "";
    }

}