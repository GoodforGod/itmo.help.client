using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using iTMO.Help.View;
using iTMO.Help.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace iTMO.Help
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            SetApplicationTopColorSchema();
            this.InitializeComponent();
        }

        private void SetApplicationTopColorSchema()
        {
            ApplicationViewTitleBar tb = ApplicationView.GetForCurrentView().TitleBar;
            Color red       = Color.FromArgb(0xff, 0xec, 0x19, 0x46);
            Color blue      = Color.FromArgb(0xff, 0x09, 0x43, 0xa0);
            Color lightBlue = Color.FromArgb(0xff, 0x9a, 0xb9, 0xea);
            Color whiteBlue = Color.FromArgb(0xff, 0xc0, 0xd0, 0xe8);
            Color white     = Colors.White;
            Color grey      = Color.FromArgb(0xff, 0xd6, 0xd6, 0xd6);

            tb.BackgroundColor              = white;
            tb.ButtonBackgroundColor        = white;
            tb.ButtonForegroundColor        = red;
            tb.ButtonHoverBackgroundColor   = grey;
            tb.ButtonHoverForegroundColor   = blue;
            tb.ForegroundColor              = blue;

            tb.ButtonPressedBackgroundColor = lightBlue;
            tb.ButtonHoverBackgroundColor   = whiteBlue;
            tb.ButtonInactiveForegroundColor = red;
            tb.InactiveBackgroundColor      = grey;
            tb.InactiveForegroundColor      = blue;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MenuListOpts.ItemsSource = MenuOptions;
        }

        private void btnBar_Click(object sender, RoutedEventArgs e)
        {
            MBar.IsPaneOpen = !MBar.IsPaneOpen;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuListOpts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            var item = list.SelectedItem as MenuItem;

            MContent.Navigate(item.Page);
        }
    }
}