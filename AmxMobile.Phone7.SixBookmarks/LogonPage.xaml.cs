using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace AmxMobile.Phone7.SixBookmarks
{
    public partial class LogonPage : PhoneApplicationPage, IContextSource
    {
        private const string UsernameKey = "Username";
        private const string PasswordKey = "Password";

        public LogonPage()
        {
            InitializeComponent();
        }

        private void buttonLogon_Click(object sender, RoutedEventArgs e)
        {
            DoLogon();
        }

        private void DoLogon()
        {
            // validate...
            ErrorBucket bucket = new ErrorBucket();
            string username = this.textUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
                bucket.Add("Username is required.");
            string password = this.textPassword.Password.Trim();
            if (string.IsNullOrEmpty(password))
                bucket.Add("Password is required.");

            // error?
            if (bucket.HasErrors)
            {
                Alert.Show(this, bucket.GetAllErrorsSeparatedByCrLf());
                return;
            }

            // clear the credentials...
            this.ClearCredentials();

            // logon...
            UsersService users = new UsersService();
            users.Logon(username, password, delegate(LogonResponse response)
            {
                // we managed to get a response...
                if (response.Result == LogonResult.LogonOk)
                {
                    // we did it...
                    this.LogonOk();
                }
                else
                    Alert.Show(this, response.Message);

            }, Alert.GetFailedHandler(this));
        }

        private void LogonOk()
        {
            // flip back - we're not on the right thread...
            this.Dispatcher.BeginInvoke(delegate()
            {
                // save...
                if (this.checkRememberMe.IsChecked.Value)
                {
                    SimpleXmlPropertyBag settings = SixBookmarksRuntime.Current.Settings;
                    settings[UsernameKey] = this.textUsername.Text.Trim();
                    settings[PasswordKey] = this.textPassword.Password.Trim();
                    settings.Save();
                }
                else
                    this.ClearCredentials();

                // do sync will come here...
                Sync sync = new Sync();
                sync.DoSync(delegate() { 

                    // we worked...
                    Dispatcher.BeginInvoke(delegate()
                    {
                        NavigationService.Navigate(new Uri("/NavigatorPage.xaml", UriKind.RelativeOrAbsolute));
                    });

                }, Alert.GetFailedHandler(this));
            });
        }

        private void ClearCredentials()
        {
            // set...
            SimpleXmlPropertyBag settings = SixBookmarksRuntime.Current.Settings;   
            if (settings.ContainsKey(UsernameKey))
                settings.Remove(UsernameKey);
            if (settings.ContainsKey(PasswordKey))
                settings.Remove(PasswordKey);

            // save...
            settings.Save();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // load the settings...
            SimpleXmlPropertyBag settings = SixBookmarksRuntime.Current.Settings;
            if (settings.ContainsKey(UsernameKey) && settings.ContainsKey(PasswordKey))
            {
                this.textUsername.Text = settings[UsernameKey];
                this.textPassword.Password = settings[PasswordKey];

                // ok...
                this.DoLogon();
            }
        }
    }
}