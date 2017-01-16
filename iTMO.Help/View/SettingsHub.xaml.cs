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
    public sealed partial class SettingsHub : Page
    {
        private User User = null;

        public SettingsHub()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            DatabaseController.Me.DUser = User;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((User = DatabaseController.Me.DUser) != null)
            {
                IsNotified.IsChecked = User.isNotified;
                AutoGroup.IsChecked = User.isAutoGroupSelect;
                AutoLogin.IsChecked = User.isAutoLogin;
                AutoTerm.IsChecked  = User.isAutoTermSelect;
                if (string.IsNullOrEmpty(User.Password))
                    Password.Password   = User.Password;
                if (string.IsNullOrEmpty(User.Login))
                    Login.Text          = User.Login;
                if (string.IsNullOrEmpty(User.Group))
                    Group.Text          = User.Group;
            }
        }

        private void IsNotified_Click(object sender, RoutedEventArgs e)
        {
            User.isNotified = (bool)IsNotified.IsChecked;
            DatabaseController.Me.DUser = User;
        }

        private void AutoTerm_Click(object sender, RoutedEventArgs e)
        {
            User.isAutoTermSelect = (bool)AutoTerm.IsChecked;
            DatabaseController.Me.DUser = User;
        }

        private void AutoGroup_Click(object sender, RoutedEventArgs e)
        {
            User.isAutoGroupSelect = (bool)AutoGroup.IsChecked;
            DatabaseController.Me.DUser = User;
        }

        private void AutoLogin_Click(object sender, RoutedEventArgs e)
        {
            User.isAutoLogin = (bool)AutoLogin.IsChecked;
            DatabaseController.Me.DUser = User;
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (IsInputValid(Group.Text))
            {
                User.Password = Password.Password;
                DatabaseController.Me.DUser = User;
            }
        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInputValid(Group.Text))
            {
                User.Login = Login.Text;
                DatabaseController.Me.DUser = User;
            }
        }

        private void Group_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInputValid(Group.Text))
            {
                User.Group = Group.Text;
                DatabaseController.Me.DUser = User;
            }
        }

        private bool IsInputValid(string text)
        {
            return !(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text));
        }
    }
}
