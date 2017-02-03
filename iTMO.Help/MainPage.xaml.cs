using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using iTMO.Help.Model;
using iTMO.Help.Controller;
using Windows.Foundation;
using System.Collections.Generic;
using iTMO.Help.View;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace iTMO.Help
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private User User = null;

        /// <summary>
        /// Main menu item's list <see cref="MenuItem"/>
        /// </summary>
        List<MenuItem> MenuOptions = new List<MenuItem>()
        {
            new MenuItem() { Page = typeof(JournalHub),     Title = "Journal",  Icon = "\xE2AC" },
            new MenuItem() { Page = typeof(ScheduleHub),    Title = "Schedule", Icon = "\xE163" },
            new MenuItem() { Page = typeof(ExamHub),        Title = "Exams",    Icon = "\xE184" },
            new MenuItem() { Page = typeof(CustomHub),      Title = "Custom",   Icon = "\xE113" },
            new MenuItem() { Page = typeof(MessageHub),     Title = "Messages", Icon = "\xE119" },
            new MenuItem() { Page = typeof(The101),         Title = "Room 101", Icon = "\xE70C" }
        };

        public MainPage()
        {
            SetApplicationTopColorSchema();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 365, Height = 380 });
            this.InitializeComponent();
        }

        private void SetApplicationTopColorSchema()
        {
            ApplicationViewTitleBar tb = ApplicationView.GetForCurrentView().TitleBar;
            Color red       = Color.FromArgb(0xff, 0xec, 0x19, 0x46);
            Color blue      = Color.FromArgb(0xff, 0x09, 0x43, 0xa0);
            Color lightBlue = Color.FromArgb(0xff, 0x9a, 0xb9, 0xea);
            Color whiteBlue = Color.FromArgb(0xff, 0xc0, 0xd0, 0xe8);
            Color white     = Colors.White;
            Color grey      = Color.FromArgb(0xff, 0xd6, 0xd6, 0xd6);

            tb.ForegroundColor              = blue;
            tb.BackgroundColor              = white;
            tb.ButtonBackgroundColor        = white;
            tb.ButtonForegroundColor        = red;

            tb.ButtonHoverBackgroundColor   = grey;
            tb.ButtonHoverForegroundColor   = blue;

            tb.ButtonPressedBackgroundColor = lightBlue;
            tb.ButtonHoverBackgroundColor   = whiteBlue;

            tb.ButtonInactiveForegroundColor = red;
            tb.ButtonInactiveBackgroundColor = grey;

            tb.InactiveBackgroundColor      = grey;
            tb.InactiveForegroundColor      = blue;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MenuListOpts.ItemsSource = MenuOptions;
            MenuListOpts.SelectedIndex = DatabaseController.Me.DUser.MenuLastSelected;
        }

        private void btnBar_Click(object sender, RoutedEventArgs e)
        {
            MBar.IsPaneOpen = !MBar.IsPaneOpen;
        }

        private void MenuListOpts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            var item = list.SelectedItem as MenuItem;
            var usr = DatabaseController.Me.DUser;
            usr.MenuLastSelected = list.SelectedIndex;
            DatabaseController.Me.DUser = usr;
            FrameContent.Navigate(item.Page);
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

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsView.IsPaneOpen = !SettingsView.IsPaneOpen && (User = DatabaseController.Me.DUser) != null)
            {
                IsNotified.IsChecked    = User.isNotified;
                AutoTerm.IsChecked      = User.isAutoTermSelect;
                if (IsInputValid(User.Password))
                    Password.Password   = User.Password;
                if (IsInputValid(User.Login))
                    Login.Text          = User.Login;
                if (IsInputValid(User.Group))
                    Group.Text          = User.Group;
            }
        }

        private void SettingsView_PaneClosed(SplitView sender, object args)
        {
            DatabaseController.Me.DUser = User;
        }
    }
}