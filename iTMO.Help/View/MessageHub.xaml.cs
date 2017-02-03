using iTMO.Help.Controller;
using iTMO.Help.Model;
using iTMO.Help.Utils;

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
        private List<MessageDe> DeMessages = null;
        private TextBlock LastSelectedDeMsg = null;

        public MessageHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((DeMessages = DatabaseController.Me.DMessageDe) != null)
                DeMsgList.ItemsSource = DeMessages;
            else
                ProcessMessageDE();
        }

        private async void ProcessMessageDE()
        {
            int days = 14;
            if (string.IsNullOrWhiteSpace(DeMsgSearch.Text) || !int.TryParse(DeMsgSearch.Text, out days))
            {
                Message.Text = "Invalid Days Input";
                return;
            }

            var user_data = await CollectUserData();
            if (!user_data.isValid)
            {
                Message.Text = user_data.Message;
                return;
            }

            Message.Text = "";
            DeMsgList.ItemsSource = DeMessages = null;

            user_data.Data.Opts.Add(DeMsgSearch.Text);

            ProgcessRing.IsActive = true;
            HttpData<string> response = await HttpController.RetrieveData(TRequest.DeMessages, user_data);

            if (response.isValid)
            {
                RememberUserData(user_data);

                var dataVR = SerializeUtils.ToMessageDeView(response.Data);

                if (dataVR.isValid)
                    DeMsgList.ItemsSource = new ObservableCollection<MessageDe>(DatabaseController.Me.DMessageDe = DeMessages = dataVR.Data);
                else
                    Message.Text = dataVR.Message;
            }
            else
                Message.Text = response.Data;

            ProgcessRing.IsActive = false;
        }

        private void RememberUserData(UserData response)
        {
            if (response.isRemember)
            {
                var usr = DatabaseController.Me.DUser;
                usr.Login = response.Data.Login;
                usr.Password = response.Data.Password;
                DatabaseController.Me.DUser = usr;
            }
        }

        private async Task<UserData> CollectUserData()
        {
            UserData response = new UserData();

            if ((string.IsNullOrWhiteSpace(response.Data.Login = DatabaseController.Me.DUser.Login)
              || string.IsNullOrWhiteSpace(response.Data.Password = DatabaseController.Me.DUser.Password)))
            {
                if (response.Data.Login != null)
                    Login.Text = response.Data.Login;
                if (response.Data.Password != null)
                    Password.Password = response.Data.Password;

                switch (await DeFormDialog.ShowAsync())
                {
                    case ContentDialogResult.Primary:
                        if (string.IsNullOrWhiteSpace(response.Data.Login = Login.Text)
                            || string.IsNullOrWhiteSpace(response.Data.Password = Password.Password))
                            response.Message = "Fill Login And Password Correctly";
                        else
                        {
                            if ((bool)RememberMeBox.IsChecked)
                                response.isRemember = true;
                            response.isValid = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else response.isValid = true;

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
            var container = ((ListViewItem)(DeMsgList.ContainerFromItem(list.SelectedItem)));

            var textBlock = (TextBlock)CommonUtils.GetChildren(container).First(x => x.Name == "Text");
            textBlock.Text = DeMessages[int.Parse(textBlock.Tag.ToString())].text;

            if(LastSelectedDeMsg != null)
                LastSelectedDeMsg.Text = "";
            LastSelectedDeMsg = textBlock;

            /*
            // WEBView Stratagy
            var webview = (WebView)GetChildren(container).First(x => x.Name == "TextWeb");
            webview.NavigateToString(MessageDE[int.Parse(webview.Tag.ToString())].text);
            */
        }
    }
}
