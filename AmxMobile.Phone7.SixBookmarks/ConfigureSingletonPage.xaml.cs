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
using Microsoft.Phone.Shell;

namespace AmxMobile.Phone7.SixBookmarks
{
    public partial class ConfigureSingletonPage : PhoneApplicationPage
    {
        private Bookmark _bookmark;

        public ConfigureSingletonPage()
        {
            InitializeComponent();

            // application bar...
            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.IsVisible = true;
            this.ApplicationBar.IsMenuEnabled = true;

            // add...
            ApplicationBarIconButton delete = new ApplicationBarIconButton(new Uri("/Images/Delete.png", UriKind.Relative));
            delete.Text = "delete";
            delete.Click += new EventHandler(delete_Click);
            this.ApplicationBar.Buttons.Add(delete);
        }

        void delete_Click(object sender, EventArgs e)
        {
            // delete...
            if (Bookmark == null)
                throw new InvalidOperationException("'Bookmark' is null.");
            this.Bookmark.IsLocalDeleted = true;
            this.Bookmark.SaveChanges();

            // navigate...
            NavigationService.Navigate(new Uri("/ConfigureForm.xaml", UriKind.RelativeOrAbsolute));
        }

        private Bookmark Bookmark
        {
            get
            {
                if (_bookmark == null)
                {
                    string asString = NavigationContext.QueryString["ordinal"];
                    int ordinal = int.Parse(asString);

                    // get...
                    _bookmark = Bookmark.GetByOrdinal(ordinal);

                    // if we don't have one, we must be new...
                    if (_bookmark == null)
                    {
                        Bookmark newBookmark = new Bookmark();
                        newBookmark.Ordinal = ordinal;
                        newBookmark.IsLocalDeleted = false;
                        newBookmark.IsLocalModified = true;

                        // set...
                        _bookmark = newBookmark;
                    }
                }
                return _bookmark;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // show...
            if (Bookmark == null)
                throw new InvalidOperationException("'Bookmark' is null.");
            this.DataContext = this.Bookmark;
        }

        private void buttonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ErrorBucket errors = new ErrorBucket();
            string name = this.textName.Text.Trim();
            if (string.IsNullOrEmpty(name))
                errors.Add("Name is required.");
            string url = this.textUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
                errors.Add("URL is required.");

            // ok?
            if(!(errors.HasErrors))
            {
                // set...
                if (Bookmark == null)
                    throw new InvalidOperationException("'Bookmark' is null.");
                this.Bookmark.Name = name;
                this.Bookmark.Url = url;

                // set...
                this.Bookmark.IsLocalModified = true;
                this.Bookmark.IsLocalDeleted = false;

                // save...
                this.Bookmark.SaveChanges();

                // back...
                NavigationService.Navigate(new Uri("/ConfigureForm.xaml", UriKind.RelativeOrAbsolute));
            }

            // show...
            if(errors.HasErrors)
                Alert.Show(this, errors.GetAllErrorsSeparatedByCrLf());
        }
    }
}