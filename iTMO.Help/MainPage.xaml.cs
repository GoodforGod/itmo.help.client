using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            this.InitializeComponent();
        }

        private void btnBar_Click(object sender, RoutedEventArgs e)
        {
            if (!MainBar.IsPaneOpen)
            {
                MainBar.IsPaneOpen = true;
                btnBar.Content = "\uE00E";
            }
            else
            {
                MainBar.IsPaneOpen = false;
                btnBar.Content = "\uE00F";
            }
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
