using iTMO.Help.Controller;
using iTMO.Help.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomHub : Page
    {
        private List<JournalCustom> JournalCustom = null;

        public CustomHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((JournalCustom = DatabaseController.Me.GetCustomJournals()) != null)
                JournalCustomList.ItemsSource = new ObservableCollection<JournalCustom>(JournalCustom);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(NameBox.Text) && !string.IsNullOrWhiteSpace(LinkBox.Text))
            {
                var temp = new JournalCustom() { Name = NameBox.Text, Link = LinkBox.Text };
                JournalCustom.Add(temp);
                DatabaseController.Me.SaveCustomJournal(temp);
            }
        }
    }
}
