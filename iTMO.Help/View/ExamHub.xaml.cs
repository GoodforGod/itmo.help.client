using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iTMO.Help.Controller;
using iTMO.Help.Model.ViewReady;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExamHub : Page
    {
        List<ExamVR> exams = null;

        public ExamHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatingFrom( NavigatingCancelEventArgs e)
        {
            DatabaseController.Me.DExams = exams;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((exams = DatabaseController.Me.DExams) != null && exams.Count != 0)
                ExamList.ItemsSource = exams;
            if (DatabaseController.Me.DUser != null && DatabaseController.Me.DUser.GroupLastUsed != null)
                SearchAutoSuggestBox.Text = DatabaseController.Me.DUser.GroupLastUsed;
        }

        private void RestorePage()
        {
            if ((exams = DatabaseController.Me.DExams) != null && exams.Count != 0)
                ExamList.ItemsSource = exams;
            if (DatabaseController.Me.DUser != null && DatabaseController.Me.DUser.GroupLastUsed != null)
                SearchAutoSuggestBox.Text = DatabaseController.Me.DUser.GroupLastUsed;
        }

        private async void ProccessExamVR()
        {
            if (string.IsNullOrWhiteSpace(SearchAutoSuggestBox.Text))
                return;

            Message.Text = "";
            ExamRing.IsActive = true;

            DataResponse<string> response = await HttpController.RetrieveData(RequestTypes.ScheduleExam, SearchAutoSuggestBox.Text);

            if (response.isValid)
            {
                var dataVR = SerializeContoller.ToExamView(response.Data);

                if (dataVR.IsValid)
                {
                    var usr = DatabaseController.Me.DUser;
                    usr.GroupLastUsed = SearchAutoSuggestBox.Text;
                    DatabaseController.Me.DUser = usr;

                    ExamList.ItemsSource = new ObservableCollection<ExamVR>(exams = dataVR.Data);
                }
                else Message.Text = dataVR.Message;
            }
            else Message.Text = response.Data;

            ExamRing.IsActive = false;
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ProccessExamVR();
        }
    }
}
