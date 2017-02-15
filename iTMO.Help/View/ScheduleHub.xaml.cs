using Windows.UI.Xaml.Controls;
using iTMO.Help.Controller;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using iTMO.Help.Model.ViewReady;
using System.Collections.Generic;
using iTMO.Help.Model;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
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
            HttpData<string> data = await HttpController.RetrieveData(TRequest.Schedule, new UserData(new List<string>() { AllSearchBox.Text }));

            if (data != null)
            {
                if (data.isValid)
                {

                }
                else
                {

                }
            }
            else
            {

            }

            return null;
        }

        private async void AllSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(AllSearchBox.Text))
                return;

            HttpData<string> data = await HttpController.RetrieveData(TRequest.Schedule, new UserData(new List<string>() { AllSearchBox.Text }));

            if (data != null)
            {
                if(data.isValid)
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void EvenSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void OddSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

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
