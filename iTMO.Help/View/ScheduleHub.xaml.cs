using Windows.UI.Xaml.Controls;
using iTMO.Help.Controller;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using iTMO.Help.Model.ViewReady;
using System.Collections.Generic;
using iTMO.Help.Model;
using System.Threading.Tasks;
using iTMO.Help.Utils;

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

            HttpData<string> response = await HttpController.RetrieveData(TRequest.Schedule, new UserData(new List<string>() { group }));

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

        private async void EvenSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(EvenSearchBox.Text))
                return;

            var schedule = Sсhedule = DatabaseController.Me.DSchedule = await RetrieveSchedule(EvenSearchBox.Text);
        }

        private async void OddSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(OddSearchBox.Text))
                return;

            var schedule = Sсhedule = DatabaseController.Me.DSchedule = await RetrieveSchedule(OddSearchBox.Text);
        }

        private List<ScheduleVR> FillScheduleType(ScheduleType type)
        {

            return null;
        }

        private void OddShareBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void EvenShareBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void AllShareBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
