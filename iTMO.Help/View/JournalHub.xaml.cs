using iTMO.Help.Controller;
using iTMO.Help.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JournalHub : Page
    {
        private Journal DJournal = null;
        private List<JournalChangeLog> DJournalChangeLog = null;

        public JournalHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if(DJournal != null)
                DatabaseController.Me.DJournal = DJournal;
            if(DJournalChangeLog != null)
                DatabaseController.Me.DJournalChangeLog = DJournalChangeLog;
            DatabaseController.Me.GroupLastSelectedIndex = GroupsBox.SelectedIndex;
            DatabaseController.Me.TermLastSelectedIndex = TermBox.SelectedIndex;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DJournal = DatabaseController.Me.DJournal;

            ProcessLastSelectedTerm();

            if (IsJournalValid())
            {
                ProcessLastSelectedGroup();
                JournalFillStrategy();
            }

            if ((DJournalChangeLog = DatabaseController.Me.DJournalChangeLog) != null)
                JournalLogList.ItemsSource = DJournalChangeLog;
        }

        private void ProcessLastSelectedGroup()
        {
            GroupsBox.ItemsSource = DJournal.years;

            var selectedGroup = DatabaseController.Me.GroupLastSelectedIndex;
            if (selectedGroup == -1)
                selectedGroup = GroupsBox.Items.Count - 1;
            GroupsBox.SelectedIndex = selectedGroup;
        }

        private void ProcessLastSelectedTerm()
        {
            var selectedTerm = DatabaseController.Me.TermLastSelectedIndex;
            if (selectedTerm == -1)
                selectedTerm = 0;
            TermBox.ItemsSource = new List<TermItem>() { new TermItem() { Term = "All" },
                                                         new TermItem() { Term = "First Part" },
                                                         new TermItem() { Term = "Last Part"  } };
            TermBox.SelectedIndex = selectedTerm;
        }

        private async void ProccessJournalVR()
        {
            JournalMessage.Text = "";

            if (JournalList.ItemsSource != null)
                JournalList.ItemsSource = DJournal = null;

            var user_data = await CollectUserData();

            if(!user_data.IsValid)
            {
                JournalMessage.Text = user_data.Message;
                return;
            }

            JournalRing.IsActive = true;

            DataResponse<string> response = await HttpController.RetrieveData(RequestTypes.Journal, 
                                                                                user_data.Login, 
                                                                                user_data.Password);
            if (response.isValid)
            {
                RememberUserData(user_data);

                var dataVR = SerializeContoller.ToJournalView(response.Data);

                if (dataVR.IsValid)
                {
                    DJournal = dataVR.Data;
                    GroupsBox.ItemsSource = DJournal.years;
                    GroupsBox.SelectedIndex = GroupsBox.Items.Count - 1;
                    //JournalList.ItemsSource = new ObservableCollection<Subject>(dataVR.Data.years[selectedGrp].subjects);
                }
                else JournalMessage.Text = dataVR.Message;
            }
            else JournalMessage.Text = response.Data;

            RememberMeBox.IsChecked = JournalRing.IsActive = false;
        }

        private async void ProccesJournalChangeLogVR()
        {
            JournalMessage.Text = "";

            if (JournalLogList.Items != null)
                JournalLogList.ItemsSource = DJournalChangeLog = null;

            var user_data = await CollectUserData();

            if(!user_data.IsValid)
            {
                JournalMessage.Text = user_data.Message;
                return;
            }

            int days = 14;

            if (int.TryParse(SearchAutoSuggestBox.Text, out days))
            {
                JournalRing.IsActive = true;

                DataResponse<string> response = await HttpController.RetrieveData(RequestTypes.JournalChangeLog, 
                                                                                    user_data.Login, 
                                                                                    user_data.Password, 
                                                                                    days.ToString());
                if (response.isValid)
                {
                    RememberUserData(user_data);

                    var dataVR = SerializeContoller.ToJournalChangeLogView(response.Data);

                    if (dataVR.IsValid)
                        JournalLogList.ItemsSource = new ObservableCollection<JournalChangeLog>(DJournalChangeLog = dataVR.Data);
                    else
                        JournalMessage.Text = dataVR.Message;
                }
                else
                { 
                    if (response.Code != System.Net.HttpStatusCode.NoContent)
                        JournalList.ItemsSource = null;
                    JournalMessage.Text = response.Data;
                }
            }
            else JournalMessage.Text = "Invalid Days Input";

            RememberMeBox.IsChecked = JournalRing.IsActive = false;
        }

        private bool IsJournalValid()
        {
            return DJournal != null
                    && DJournal.years != null
                    && DJournal.years.Count != 0;
        }

        private void JournalFillStrategy()
        {
            switch (TermBox.SelectedIndex)
            {
                case 1:
                    JournalList.ItemsSource
                    = new ObservableCollection<Subject>(DJournal.years[GroupsBox.SelectedIndex]
                        .subjects.GetRange(0, DJournal.years[GroupsBox.SelectedIndex].subjects.Count / 2 - 1));
                    return; 
                case 2:
                    JournalList.ItemsSource
                    = new ObservableCollection<Subject>(DJournal.years[GroupsBox.SelectedIndex]
                        .subjects.GetRange(DJournal.years[GroupsBox.SelectedIndex].subjects.Count / 2,
                            DJournal.years[GroupsBox.SelectedIndex].subjects.Count / 2 - 1));
                   return;
                default:
                    JournalList.ItemsSource
                   = new ObservableCollection<Subject>(DJournal.years[GroupsBox.SelectedIndex].subjects);
                    return;

            }
        }

        private void RememberUserData(CheckResponse response)
        {
            if (response.IsRemember)
            {
                var usr = DatabaseController.Me.DUser;
                usr.Login = response.Login;
                usr.Password = response.Password;
                DatabaseController.Me.DUser = usr;
            }
        }

        private async Task<CheckResponse> CollectUserData()
        {
            CheckResponse response = new CheckResponse();

            if ((string.IsNullOrWhiteSpace(response.Login = DatabaseController.Me.DUser.Login)
              || string.IsNullOrWhiteSpace(response.Password = DatabaseController.Me.DUser.Password)))
            {
                if (response.Login != null)
                    Login.Text = response.Login;
                if (response.Password != null)
                    Password.Password = response.Password;

                switch (await JournalFormDialog.ShowAsync())
                {
                    case ContentDialogResult.Primary:
                        if (string.IsNullOrWhiteSpace(response.Login = Login.Text)
                            || string.IsNullOrWhiteSpace(response.Password = Password.Password))
                            response.Message = "Fill Login And Password Correctly";
                        else
                        {
                            if ((bool)RememberMeBox.IsChecked)
                                response.IsRemember = true;
                            response.IsValid = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else response.IsValid = true;

            Login.Text = Password.Password = "";
            return response;
        }

        private void TermBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsJournalValid())
                JournalFillStrategy();
        }

        private void GroupsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsJournalValid() && DJournal.years.Count >= GroupsBox.SelectedIndex)
                JournalFillStrategy();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProccessJournalVR();
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ProccesJournalChangeLogVR(); 
        }
    }
}
