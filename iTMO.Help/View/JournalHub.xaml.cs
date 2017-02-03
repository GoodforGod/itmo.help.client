using iTMO.Help.Controller;
using iTMO.Help.Model;
using iTMO.Help.Utils;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private List<JournalChangeLog> DJournalChange = null;
        private Grid LastSelected = null;

        public JournalHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DJournal = DatabaseController.Me.DJournal;

            ProcessLastSelectedTerm();

            if (IsJournalValid())
                ProcessLastSelectedGroup();
            else
                ProccessJournalVR();
        }

        private void ProcessLastSelectedGroup()
        {
            GroupsBox.ItemsSource = DJournal.years;
            GroupsBox.SelectedIndex = GroupsBox.Items.Count - 1;
        }

        private void ProcessLastSelectedTerm()
        {
            TermBox.ItemsSource = new List<TermItem>()
            {
                new TermItem() { Term = "All" },
                new TermItem() { Term = "First" },
                new TermItem() { Term = "Last"  }
            };
            TermBox.SelectedIndex = DatabaseController.Me.TermLastSelectedIndex; ;
        }

        private async void ProccessJournalVR()
        {
            var user_data = await CollectUserData();
            if(!user_data.isValid)
            {
                JournalMessage.Text = user_data.Message;
                return;
            }

            JournalMessage.Text = "";
            JournalList.ItemsSource = DJournal = null;

            JournalRing.IsActive = true;
            HttpData<string> response = await HttpController.RetrieveData(TRequest.DeJournal, user_data);

            if (response.isValid)
            {
                RememberUserData(user_data);

                var dataVR = SerializeUtils.ToJournalView(response.Data);

                if (dataVR.isValid)
                {
                    DatabaseController.Me.DJournal = DJournal = dataVR.Data;
                    GroupsBox.ItemsSource = DJournal.years;
                    GroupsBox.SelectedIndex = GroupsBox.Items.Count - 1;
                }
                else
                {
                    JournalMessage.Text = dataVR.Message;
                    RefreshBtn.IsEnabled = true;
                }
            }
            else
            {
                JournalMessage.Text = response.Data;
                RefreshBtn.IsEnabled = true;
            }

            RememberMeBox.IsChecked = JournalRing.IsActive = false;
        }

        private async void ProccesJournalChangeLogVR()
        {
            int days = 14;
            if (string.IsNullOrWhiteSpace(ChangeLogSearch.Text) || !int.TryParse(ChangeLogSearch.Text, out days))
            {
                JournalChangeMessage.Text = "Invalid Days Input";
                return;
            }

            var user_data = await CollectUserData();
            if(!user_data.isValid)
            {
                JournalMessage.Text = user_data.Message;
                return;
            }

            JournalChangeMessage.Text = "";
            JournalChangeList.ItemsSource = DJournalChange = null;

            user_data.Data.Opts.Add(ChangeLogSearch.Text);

            JournalRing.IsActive = true;
            HttpData<string> response = await HttpController.RetrieveData(TRequest.DeJournalChangeLog, user_data);

            if (response.isValid)
            {
                RememberUserData(user_data);

                var dataVR = SerializeUtils.ToJournalChangeLogView(response.Data);

                if (dataVR.isValid)
                    JournalChangeList.ItemsSource = new ObservableCollection<JournalChangeLog>(DatabaseController.Me.DJournalChangeLog = DJournalChange = dataVR.Data);
                else
                    JournalMessage.Text = dataVR.Message;
            }
            else
                JournalChangeMessage.Text = response.Data;

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

        private void RememberUserData(UserData response)
        {
            if (response.isRemember)
            {
                var usr = DatabaseController.Me.DUser;
                usr.Login = response.Data.Login;
                usr.Password = response.Data.Password;
                DatabaseController.Me.DUser = usr;
            }
        }

        private async Task<UserData> CollectUserData()
        {
            UserData response = new UserData();

            if ((string.IsNullOrWhiteSpace(response.Data.Login = DatabaseController.Me.DUser.Login)
              || string.IsNullOrWhiteSpace(response.Data.Password = DatabaseController.Me.DUser.Password)))
            {
                if (response.Data.Login != null)
                    Login.Text = response.Data.Login;
                if (response.Data.Password != null)
                    Password.Password = response.Data.Password;

                switch (await JournalFormDialog.ShowAsync())
                {
                    case ContentDialogResult.Primary:
                        if (string.IsNullOrWhiteSpace(response.Data.Login = Login.Text)
                            || string.IsNullOrWhiteSpace(response.Data.Password = Password.Password))
                            response.Message = "Fill Login And Password Correctly";
                        else
                        {
                            if ((bool)RememberMeBox.IsChecked)
                                response.isRemember = true;
                            response.isValid = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else response.isValid = true;

            Login.Text = Password.Password = "";
            return response;
        }

        private void TermBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsJournalValid() && GroupsBox.SelectedIndex != -1)
            {
                DatabaseController.Me.TermLastSelectedIndex = TermBox.SelectedIndex;
                JournalFillStrategy();
            }
        }

        private void GroupsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsJournalValid() && DJournal.years.Count >= GroupsBox.SelectedIndex)
            {
                DatabaseController.Me.GroupLastSelectedIndex = GroupsBox.SelectedIndex;
                JournalFillStrategy();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshBtn.IsEnabled = false;
            ProccessJournalVR();
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ProccesJournalChangeLogVR(); 
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = (PivotItem)(sender as Pivot).SelectedItem;
            if (pivot.Header.ToString() == "Changes")
            {
                if ((DJournalChange = DatabaseController.Me.DJournalChangeLog) != null)
                    JournalChangeList.ItemsSource = DJournalChange;
                else
                    ProccesJournalChangeLogVR();
            }
        }

        private void JournalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            var listItem = list.SelectedItem as DependencyObject;
            var container = ((ListViewItem)(JournalList.ContainerFromItem(list.SelectedItem)));

            if (LastSelected != null)
                LastSelected.Visibility = Visibility.Collapsed;

            var stackpanel = LastSelected = (Grid)CommonUtils.GetChildren(container).First(x => x.Name == "Info");
            stackpanel.Visibility = Visibility.Visible;
        }

        private void JournalList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView list = sender as ListView;
            ListViewItem listItem = list.ContainerFromItem(e.ClickedItem) as ListViewItem;

            if (listItem.IsSelected)
            {
                listItem.IsSelected = false;
                list.SelectionMode = ListViewSelectionMode.None;
            }
            else
            {
                listItem.IsSelected = true;
            }
        }
    }
}
