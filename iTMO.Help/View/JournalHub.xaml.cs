using iTMO.Help.Controller;
using iTMO.Help.Model;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JournalHub : Page
    {
        private Journal Journal = null;

        public JournalHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            DatabaseController.Me.DJournal = Journal;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            FullFillPage();
        }

        private void FullFillPage()
        {
            if (DatabaseController.Me.DJournal != null)
            {
                //ExamList.ItemsSource = Journal = DatabaseController.Me.DJournal;
                JournalRing.IsActive = false;
                JournalLogRing.IsActive = false;
            }
        }

        private async void ProccessJournal()
        {
            DataResponse response = await HttpController.RetrieveData(RequestTypes.Journal, "","");

            if (response.isValid)
            {
                var exams = SerializeContoller.ToExamViewReady(response.Data);
                //ExamList.ItemsSource = exams.Data;
            }
            else
            {
                JournalMessage.Text     = response.Data;
                JournalLogMessage.Text  = response.Data;
            }
        }
    }
}
