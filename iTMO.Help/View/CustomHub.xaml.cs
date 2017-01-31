using iTMO.Help.Controller;
using iTMO.Help.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
            ProcessCustomJournal();
        }

        private void ProcessCustomJournal()
        {
            if ((DatabaseController.Me.GetCustomJournals()) != null)
                JournalCustomList.ItemsSource = new ObservableCollection<JournalCustom>(JournalCustom = DatabaseController.Me.GetCustomJournals());
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(NameBox.Text) && !string.IsNullOrWhiteSpace(LinkBox.Text))
            {
                DatabaseController.Me.SaveCustomJournal(new JournalCustom() { Name = NameBox.Text, Link = LinkBox.Text });
                NameBox.Text = LinkBox.Text = "";
                ProcessCustomJournal();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DatabaseController.Me.DeleteCustomJournal(int.Parse((sender as Button).Tag.ToString()));
            ProcessCustomJournal();
        }

        private async void JournalCustomList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var journal = e.ClickedItem as JournalCustom;

            try                       { var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(journal.Link)); }
            catch(UriFormatException) { journal.Name = "INVALID"; }
            catch(Exception ex)       { }
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            if (JournalCustom == null || JournalCustom.Count == 0)
                return;

            var data = new DataPackage();
            var targetId = int.Parse((sender as Button).Tag.ToString());
            data.SetText(JournalCustom.Find(x => x.Id == targetId) .Link);
            Clipboard.SetContent(data);
        }
    }
}
