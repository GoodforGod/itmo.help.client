using Windows.UI.Xaml.Controls;
using iTMO.Help.Controller;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScheduleHub : Page
    {
        public ScheduleHub()
        {
            this.InitializeComponent();
        }

        private async void AllSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(AllSearchBox.Text))
                return;

            DataResponse<string> data = await HttpController.RetrieveData(RequestTypes.Schedule, AllSearchBox.Text);

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
    }
}
