using Windows.UI.Xaml.Controls;
using iTMO.Help.Controller;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using iTMO.Help.Model;
using System.Threading.Tasks;
using iTMO.Help.Utils;
using Windows.UI.Xaml;
using System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    enum ScheduleType
    {
        ALL,
        ODD,
        EVEN
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScheduleHub : Page
    {
        private List<ScheduleVR> Sсhedules = null;
        private ListView lastSelectedLessons = null;

        public ScheduleHub()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ProcessWeekPart();

            if ((Sсhedules = DatabaseController.Me.DSchedule) != null)
                ScheduleList.ItemsSource = Sсhedules;
            else if(!string.IsNullOrWhiteSpace(AllSearchBox.Text = GetUserGroup()))
                ScheduleList.ItemsSource = Sсhedules = DatabaseController.Me.DSchedule = await RetrieveSchedule(AllSearchBox.Text);
        }

        private string GetUserGroup()
        {
            string grp = DatabaseController.Me.DUser.Group;

            if (string.IsNullOrWhiteSpace(grp))
                grp = DatabaseController.Me.DUser.GroupLastUsed;

            return grp;
        }

        private async Task<List<ScheduleVR>> RetrieveSchedule(string group)
        {
            Message.Text = "";
            ProgressRing.IsActive = true;
            List<ScheduleVR> scheduleReceived = new List<ScheduleVR>();

            HttpData<string> response = await HttpController.RetrieveData(TRequest.Schedule, new UserData(group));

            if (response.isValid)
            {
                var dataVR = SerializeUtils.ToScheduleView(response.Data);

                if (dataVR.isValid)
                    scheduleReceived = dataVR.Data;
                else Message.Text = dataVR.Message;
            }
            else Message.Text = response.Data;

            ProgressRing.IsActive = false;
            return scheduleReceived;
        }

        private void ProcessWeekPart()
        {
            WeekBox.ItemsSource = new List<WeekItem>()
            {
                new WeekItem() { WeekPart = "All" },
                new WeekItem() { WeekPart = "Odd" },
                new WeekItem() { WeekPart = "Even"  }
            };
        }

        private async void AllSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(AllSearchBox.Text))
                return;

            var schedule = Sсhedules = DatabaseController.Me.DSchedule = await RetrieveSchedule(AllSearchBox.Text);

            ScheduleList.ItemsSource = schedule;
        }

        private List<ScheduleVR> FillScheduleType(ScheduleType type, List<ScheduleVR> schedule)
        {

            return null;
        }

        private void TeacherHyper_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void AllShareBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScheduleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var list = sender as ListView;
                var listItem = list.SelectedItem as DependencyObject;
                var container = ((ListViewItem)(ScheduleList.ContainerFromItem(list.SelectedItem)));

                var weekday = (TextBlock)CommonUtils.GetChildren(container).Find(x => x.Name == "Day");
                var lessons = (ListView)CommonUtils.GetChildren(container).Find(x => x.Name == "Lessons");
                lessons.ItemsSource = Sсhedules[int.Parse(weekday.Tag.ToString()) - 1].Lessons;

                if (lastSelectedLessons != null)
                    lastSelectedLessons.ItemsSource = new List<ScheduleVR>();
                lastSelectedLessons = lessons;
            }
            catch(Exception ex)
            {
                Message.Text = ex.Message;
            }
        }

        private void WeekBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
