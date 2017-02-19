using Windows.UI.Xaml.Controls;
using iTMO.Help.Controller;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using iTMO.Help.Model.ViewReady;
using System.Collections.Generic;
using iTMO.Help.Model;
using System.Threading.Tasks;
using iTMO.Help.Utils;
using Windows.UI.Xaml;

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
        private List<ScheduleVR> Sсhedule = null;

        public ScheduleHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if((Sсhedule = DatabaseController.Me.DSchedule) != null)
            {

            }
        }

        private async Task<List<ScheduleVR>> RetrieveSchedule(string group)
        {
            Message.Text = "";
            ProgressRing.IsActive = true;
            List<ScheduleVR> scheduleReceived = null;

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

        private async void AllSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(AllSearchBox.Text))
                return;

            var schedule = Sсhedule = DatabaseController.Me.DSchedule = await RetrieveSchedule(AllSearchBox.Text);
        }

        private List<ScheduleVR> FillScheduleType(ScheduleType type, List<ScheduleVR> schedule)
        {

            return null;
        }

        private void OddShareBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EvenShareBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AllShareBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WeekBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
