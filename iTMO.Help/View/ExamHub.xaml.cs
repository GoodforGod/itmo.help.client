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
using iTMO.Help.Controller;
using Windows.UI;
using System.Threading.Tasks;
using iTMO.Help.Model.ViewReady;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExamHub : Page
    {
        List<ExamVR> exams = new List<ExamVR>();

        public ExamHub()
        {
            this.InitializeComponent();
            RestorePage();
        }

        protected override void OnNavigatingFrom( NavigatingCancelEventArgs e)
        {
            DatabaseController.Me.DExams = exams;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RestorePage();
        }

        private void RestorePage()
        {
            if (DatabaseController.Me.DExams != null && DatabaseController.Me.DExams.Count != 0)
            {
                ExamList.ItemsSource = exams = DatabaseController.Me.DExams;
                ExamRing.IsActive = false;
            }
            if (DatabaseController.Me.DUser != null && DatabaseController.Me.DUser.GroupLastUsed != null)
                SearchAutoSuggestBox.Text = DatabaseController.Me.DUser.GroupLastUsed;
        }

        private async void ProccessExamVR()
        {
            if (string.IsNullOrEmpty(SearchAutoSuggestBox.Text) || string.IsNullOrWhiteSpace(SearchAutoSuggestBox.Text))
                return;

            Message.Text = "";
            ExamRing.IsActive = true;

            DataResponse<string> response = await HttpController.RetrieveData(RequestTypes.ScheduleExam, SearchAutoSuggestBox.Text);

            if (response.isValid)
            {
                var dataVR = SerializeContoller.ToExamViewReady(response.Data);

                if (dataVR.IsValid)
                {
                    var usr = DatabaseController.Me.DUser;
                    usr.GroupLastUsed = SearchAutoSuggestBox.Text;
                    DatabaseController.Me.DUser = usr;

                    ExamList.ItemsSource = exams = dataVR.Data;
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
