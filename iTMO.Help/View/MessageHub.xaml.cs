using iTMO.Help.Controller;
using iTMO.Help.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class MessageHub : Page
    {
        private List<MessageDe> MessageDE = null;
        private TextBlock LastSelected = null;

        public MessageHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((MessageDE = DatabaseController.Me.DMessageDe) != null)
                DEList.ItemsSource = MessageDE;
            else
                ProcessMessageDE();
        }

        private async void ProcessMessageDE()
        {
            if (string.IsNullOrWhiteSpace(DeSearch.Text))
                return;

            var user_data = await CollectUserData();

            if (!user_data.IsValid)
            {
                Message.Text = user_data.Message;
                return;
            }

            DEList.ItemsSource = null;
            Message.Text = "";
            ProcessRing.IsActive = true;

            DataResponse<string> response = await HttpController.RetrieveData(RequestTypes.MessagesFromDe, 
                                                                                            user_data.Login,
                                                                                            user_data.Password,
                                                                                            DeSearch.Text);
            if (response.isValid)
            {
                var dataVR = SerializeContoller.ToMessageDeView(response.Data);

                if (dataVR.IsValid)
                {
                    RememberUserData(user_data);
                    DEList.ItemsSource = new ObservableCollection<MessageDe>(DatabaseController.Me.DMessageDe = MessageDE = dataVR.Data);
                }
                else
                    Message.Text = dataVR.Message;
            }
            else Message.Text = response.Data;

            ProcessRing.IsActive = false;
        }

        private void RememberUserData(CheckResponse response)
        {
            if (response.IsRemember)
            {
                var usr = DatabaseController.Me.DUser;
                usr.Login = response.Login;
                usr.Password = response.Password;
                DatabaseController.Me.DUser = usr;
            }
        }

        private async Task<CheckResponse> CollectUserData()
        {
            CheckResponse response = new CheckResponse();

            if ((string.IsNullOrWhiteSpace(response.Login = DatabaseController.Me.DUser.Login)
              || string.IsNullOrWhiteSpace(response.Password = DatabaseController.Me.DUser.Password)))
            {
                if (response.Login != null)
                    Login.Text = response.Login;
                if (response.Password != null)
                    Password.Password = response.Password;

                switch (await DeFormDialog.ShowAsync())
                {
                    case ContentDialogResult.Primary:
                        if (string.IsNullOrWhiteSpace(response.Login = Login.Text)
                            || string.IsNullOrWhiteSpace(response.Password = Password.Password))
                            response.Message = "Fill Login And Password Correctly";
                        else
                        {
                            if ((bool)RememberMeBox.IsChecked)
                                response.IsRemember = true;
                            response.IsValid = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else response.IsValid = true;

            Login.Text = Password.Password = "";
            return response;
        }

        private void IsuSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void DeSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ProcessMessageDE();
        }

        private void Text_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void DEList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            var listItem = list.SelectedItem as DependencyObject;
            var container = ((ListViewItem)(DEList.ContainerFromItem(list.SelectedItem)));

            var textBlock = (TextBlock)GetChildren(container).First(x => x.Name == "Text");
            textBlock.Text = MessageDE[int.Parse(textBlock.Tag.ToString())].text;

            if(LastSelected != null)
                LastSelected.Text = "";
            LastSelected = textBlock;
            /*
            // WEBView Stratagy
            var webview = (WebView)GetChildren(container).First(x => x.Name == "TextWeb");
            webview.NavigateToString(MessageDE[int.Parse(webview.Tag.ToString())].text);
            */
        }

        private List<FrameworkElement> GetChildren(DependencyObject parent)
        {
            List<FrameworkElement> controls = new List<FrameworkElement>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement)
                {
                    controls.Add(child as FrameworkElement);
                }
                controls.AddRange(GetChildren(child));
            }

            return controls;
        }
    }
}
