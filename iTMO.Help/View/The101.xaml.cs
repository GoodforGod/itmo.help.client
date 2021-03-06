﻿using iTMO.Help.Controller;
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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTMO.Help.Model
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class The101 : Page
    {
        public The101()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private async void NavigateWeb()
        {
            var user_data = DatabaseController.Me.DUser;
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri("http://de.ifmo.ru/?node=schedule&index=sched&semiId=1&group=" + user_data.Group));

            /*
            ProgressRing.IsActive = true;

            //Web.Navigate(new Uri("http://de.ifmo.ru/?node=schedule&index=sched&semiId=1&group=P3100"));

            var user_data = new UserData();
            user_data.Data.Group = "P3100";
            user_data.Data.Opts.Add("1");

            HttpData<string> response = await HttpController.RetrieveData(TRequest.DeAttestationSchedule, user_data);

            if (response.isValid)
            {
                RememberUserData(user_data);

                var dataVR = SerializeUtils.ToAttestationDeView(response.Data, 1);

                if (dataVR.isValid)
                {
                    user_data = new UserData();
                    user_data.Data.Opts.Add(dataVR.Data.First.Subjects[1].Tests[1].Link);

                    HttpData<string> nodeResponse
                        = await HttpController.RetrieveData(TRequest.DeAttestationScheduleNode, user_data);

                    if(nodeResponse.isValid)
                    {

                        //var nodeVR

                    }

                }


                //if (dataVR.isValid)
                //    DeMsgList.ItemsSource = new ObservableCollection<MessageDe>(DatabaseController.Me.DMessageDe = DeMessages = dataVR.Data);
                //else
                //    Message.Text = dataVR.Message;
            }
            else
                Message.Text = response.Message;

            ProgressRing.IsActive = false;
            */
        }

        /* private void RememberUserData(UserData response)
        {
            if (response.isRemember)
            {
                var usr = DatabaseController.Me.DUser;
                usr.Login = response.Data.Login;
                usr.Password = response.Data.Password;
                DatabaseController.Me.DUser = usr;
            }
        }
        */

        /* private async Task<UserData> CollectUserData()
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
        */

        private void DE101_Click(object sender, RoutedEventArgs e)
        {
            NavigateWeb();
        }

        private async void Rank_Click(object sender, RoutedEventArgs e)
        {
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri("http://de.ifmo.ru/?node=rating"));
        }
    }
}
