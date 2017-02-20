using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iTMO.Help.Controller;
using iTMO.Help.Utils;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using iTMO.Help.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExamHub : Page
    {
        List<ExamVR> exams = null;
        private User userData = null;

        public ExamHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RestorePage();

            if (ExamList.ItemsSource == null)
                ProccessExamVR();
        }

        private void RestorePage()
        {
             if ((exams = DatabaseController.Me.DExams) != null && exams.Count != 0)
                ExamList.ItemsSource = exams;

            if ((userData = DatabaseController.Me.DUser) != null && string.IsNullOrWhiteSpace(userData.Group))
                SearchAutoSuggestBox.Text = userData.Group;
            else if(DatabaseController.Me.DUser.GroupLastUsed != null)
                SearchAutoSuggestBox.Text = DatabaseController.Me.DUser.GroupLastUsed;
        }

        private async void ProccessExamVR()
        {
            if (string.IsNullOrWhiteSpace(SearchAutoSuggestBox.Text))
                return;

            Message.Text = "";
            ExamRing.IsActive = true;
            ExamList.ItemsSource = null;

            HttpData<string> response = await HttpController.RetrieveData(TRequest.ScheduleExam, new UserData(SearchAutoSuggestBox.Text));

            if (response.isValid)
            {
                var dataVR = SerializeUtils.ToExamView(response.Data);

                if (dataVR.isValid)
                {
                    var usr = DatabaseController.Me.DUser;
                    usr.GroupLastUsed = SearchAutoSuggestBox.Text;
                    DatabaseController.Me.DUser = usr;

                    ExamList.ItemsSource = new ObservableCollection<ExamVR>(DatabaseController.Me.DExams = exams = dataVR.Data);
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

        private void ShareBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TeacherHyper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var textblock = sender as HyperlinkButton;
            if (textblock.Tag.ToString() == "")
            {

            }
        }
    }
}
