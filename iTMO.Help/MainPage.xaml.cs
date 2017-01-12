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
            ChangeApplicationColorSchema();

            this.InitializeComponent();
        }

        private void ChangeApplicationColorSchema()
        {
            ApplicationViewTitleBar tb = ApplicationView.GetForCurrentView().TitleBar;

            Color red       = Color.FromArgb(0xff, 0xec, 0x19, 0x46);
            Color blue      = Color.FromArgb(0xff, 0x1e, 0x51, 0xa4);
            Color white     = Colors.White;
            Color grey = Color.FromArgb(0xff, 0xd6, 0xd6, 0xd6);
            tb.BackgroundColor              = white;
            tb.ButtonBackgroundColor        = white;
            tb.ButtonForegroundColor        = red;
            tb.ButtonHoverBackgroundColor   = grey;
            tb.ButtonHoverForegroundColor   = blue;
            tb.ForegroundColor              = blue;

            tb.ButtonInactiveBackgroundColor = grey;
            tb.ButtonInactiveForegroundColor = red;
            tb.InactiveBackgroundColor      = grey;
            tb.InactiveForegroundColor      = blue;
        }

        private void btnBar_Click(object sender, RoutedEventArgs e)
        {
            MainBar.IsPaneOpen = !MainBar.IsPaneOpen;
        }

        private void btnRestartSession_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnJournal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTimeTable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn101_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTermTimeTable_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
