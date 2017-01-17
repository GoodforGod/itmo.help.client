using iTMO.Help.Controller;
using iTMO.Help.Model;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JournalHub : Page
    {
        private Journal Journal = null;

        public JournalHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            DatabaseController.Me.DJournal = Journal;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RestorePage();
        }

        private void RestorePage()
        {
            if ((Journal = DatabaseController.Me.DJournal) != null)
            {
                JournalList.ItemsSource = Journal.years[TermBox.SelectedIndex].subjects;
                JournalRing.IsActive = false;
            }
        }

        private async void ProccessJournalVR()
        {
            string login = null;
            string pass = null;

            JournalMessage.Text = "";

            if ((string.IsNullOrEmpty(login = DatabaseController.Me.DUser.Login)
                || string.IsNullOrEmpty(pass = DatabaseController.Me.DUser.Password)))
            {
                var result = await JournalFormDialog.ShowAsync();

                if (string.IsNullOrEmpty(login = Login.Text) 
                    || string.IsNullOrWhiteSpace(pass = Password.Password))
                {
                    JournalMessage.Text = "Fill Login And Password Correctly";
                    return;
                }
            }

            JournalRing.IsActive = true;

            DataResponse<string> response = await HttpController.RetrieveData(RequestTypes.Journal, login, pass);

            if (response.isValid)
            {
                var dataVR = SerializeContoller.ToJournalView(response.Data);

                if (dataVR.IsValid)
                {
                    Journal = dataVR.Data;
                    JournalList.ItemsSource = dataVR.Data.years[TermBox.SelectedIndex].subjects;
                }
                else JournalMessage.Text = dataVR.Message;
            }
            else JournalMessage.Text = response.Data;

            Login.Text = Password.Password = "";

            JournalRing.IsActive = false;
        }

        private void TermBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GroupsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProccessJournalVR();
        }

        private void JournalFormDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void JournalFormDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            JournalRing.IsActive = false;
        }
    }
}
