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
    public partial class NavigatorPage : PhoneApplicationPage, IContextSource
    {
        private List<Bookmark> _bookmarks;

        public NavigatorPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // get a list of bookmarks and bind it...
            this.Bookmarks = Bookmark.GetBookmarksForDisplay();
        }

        private List<Bookmark> Bookmarks
        {
            get
            {
                return _bookmarks;
            }
            set
            {
                _bookmarks = value;
                this.RefreshView();
            }
        }

        private void RefreshView()
        {
            // reset...
            for (int index = 0; index < 5; index++)
                ResetButton(index);

            // configure...
            foreach (Bookmark bookmark in this.Bookmarks)
                ConfigureBookmark(bookmark);
        }

        private void ConfigureBookmark(Bookmark bookmark)
        {
            if (bookmark == null)
                throw new ArgumentNullException("bookmark");

            // get...
            Button button = this.GetButton(bookmark.Ordinal);
            if (button == null)
                throw new InvalidOperationException("'button' is null.");

            // set it...
            button.Content = bookmark.Name;
        }

        private void ResetButton(int ordinal)
        {
            Button button = this.GetButton(ordinal);
            button.Content = "...";
        }

        private Button GetButton(int ordinal)
        {
            if (ordinal == 0)
                return this.buttonNavigate1;
            else if (ordinal == 1)
                return this.buttonNavigate2;
            else if (ordinal == 2)
                return this.buttonNavigate3;
            else if (ordinal == 3)
                return this.buttonNavigate4;
            else if (ordinal == 4)
                return this.buttonNavigate5;
            else if (ordinal == 5)
                return this.buttonNavigate6;
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", ordinal));
        }

        private void buttonConfigure_Click(object sender, RoutedEventArgs e)
        {
            HandleConfigure();
        }

        private void buttonLogoff_Click(object sender, RoutedEventArgs e)
        {
            Alert.Show(this, "TBD.");
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("http://www.multimobiledevelopment.com/"));
        }

        private void buttonNavigate_Click(object sender, RoutedEventArgs e)
        {
            int ordinal = GetOrdinalForButton((Button)sender);

            // find it...
            Bookmark bookmark = GetBookmarkForOrdinal(ordinal);
            if (bookmark != null)
                NavigationService.Navigate(new Uri(bookmark.Url));
            else
                HandleConfigure();
        }

        private int GetOrdinalForButton(Button button)
        {
            if (button == buttonNavigate1)
                return 0;
            else if (button == buttonNavigate2)
                return 1;
            else if (button == buttonNavigate3)
                return 2;
            else if (button == buttonNavigate4)
                return 3;
            else if (button == buttonNavigate5)
                return 4;
            else if (button == buttonNavigate6)
                return 5;
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", button.Name));
        }

        private Bookmark GetBookmarkForOrdinal(int ordinal)
        {
            foreach (Bookmark bookmark in this.Bookmarks)
            {
                if (bookmark.Ordinal == ordinal)
                    return bookmark;
            }

            // return...
            return null;
        }

        private void HandleConfigure()
        {
            NavigationService.Navigate(new Uri("/ConfigurePage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}