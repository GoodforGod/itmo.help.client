using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using iTMO.Help.Model;
using iTMO.Help.View;
using Windows.UI.Xaml;

namespace iTMO.Help
{
    public sealed partial class MainPage : Page
    {
        List<MenuItem> MenuOptions = new List<MenuItem>()
        {
            new MenuItem() { Page = typeof(JournalHub),     Title = "Journal",  Icon = "\xE2AC" },
            new MenuItem() { Page = typeof(ScheduleHub),    Title = "Schedule", Icon = "\xE163" },
            new MenuItem() { Page = typeof(ExamHub),        Title = "Exams",    Icon = "\xE184" },
            new MenuItem() { Page = typeof(CustomHub),      Title = "Custom",   Icon = "\xE113" },
            new MenuItem() { Page = typeof(MessageHub),     Title = "Messages", Icon = "\xE119" },
            new MenuItem() { Page = typeof(The101),         Title = "Room 101", Icon = "\xE70C" }
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
        public bool     IsRemember { get; set; } = false;
        public bool     IsValid    { get; set; } = false;
        public string   Login      { get; set; }
        public string   Password   { get; set; }
        public string   Message    { get; set; } = "";
    }

    class CustomProperties
    {
        // "HtmlString" attached property for a WebView
        public static readonly DependencyProperty HtmlStringProperty =
            DependencyProperty.RegisterAttached("HtmlString", 
                                               typeof(string), 
                                               typeof(CustomProperties), 
                                               new PropertyMetadata("", OnHtmlStringChanged));
        // Getter and Setter
        public static string GetHtmlString(DependencyObject obj)                { return (string)obj.GetValue(HtmlStringProperty); }
        public static void SetHtmlString(DependencyObject obj, string value)    { obj.SetValue(HtmlStringProperty, value); }

        // Handler for property changes in the DataContext : set the WebView
        private static void OnHtmlStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView wv = d as WebView;
            if (wv != null)
                wv.NavigateToString((string)e.NewValue);
        }
    }
}
