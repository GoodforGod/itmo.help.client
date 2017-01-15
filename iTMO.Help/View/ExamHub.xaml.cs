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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExamHub : Page
    {
        DataResponse response = null;

        public ExamHub()
        {
            this.InitializeComponent();
        }

        private async void test_Click(object sender, RoutedEventArgs e)
        {
            if(response == null)
                response = await HttpController.RetrieveData(RequestTypes.ScheduleExam, "P3310");

            if(response.isValid)
            {
                var list = SerializeContoller.ToExamViewReady(response.Data);
                Count.Text = "Amount : "  + list.Count;
                ExamList.ItemsSource = list;
            }
            else
            {
                Count.Text = "Error : Invalid Response";
            }
        }
    }
}
