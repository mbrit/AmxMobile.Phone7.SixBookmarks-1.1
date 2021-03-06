﻿using System;
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
    public partial class ConfigurePage : PhoneApplicationPage, IContextSource
    {
        public ConfigurePage()
        {
            InitializeComponent();

            // application bar...
            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.IsVisible = true;
            this.ApplicationBar.IsMenuEnabled = true;

            // add...
            ApplicationBarIconButton add = new ApplicationBarIconButton(new Uri("/Images/Add.png", UriKind.Relative));
            add.Text = "add";
            add.Click += new EventHandler(add_Click);
            this.ApplicationBar.Buttons.Add(add);

            // finish...
            ApplicationBarIconButton finish = new ApplicationBarIconButton(new Uri("/Images/Finish.png", UriKind.Relative));
            finish.Text = "finish";
            finish.Click += new EventHandler(finish_Click);
            this.ApplicationBar.Buttons.Add(finish);
        }

        void finish_Click(object sender, EventArgs e)
        {
            // sync...
            Sync sync = new Sync();
            sync.DoSync((Action)delegate()
            {
                Dispatcher.BeginInvoke((Action)delegate()
                {
                    NavigationService.Navigate(new Uri("/NavigatorPage.xaml", UriKind.RelativeOrAbsolute));
                });

            }, Alert.GetFailedHandler(this));
        }

        void add_Click(object sender, EventArgs e)
        {
            // get the next ordinal...
            bool[] taken = new bool[6];
            foreach (Bookmark bookmark in this.listBookmarks.Items)
                taken[bookmark.Ordinal] = true;

            // walk...
            for (int index = 0; index < taken.Length; index++)
            {
                if (!(taken[index]))
                {
                    ConfigureBookmark(index);
                    return;
                }
            }

            // show...
            MessageBox.Show("There are no more slots available.");
        }

        private void ConfigureBookmark(int ordinal)
        {
            NavigationService.Navigate(new Uri(string.Format("/ConfigureSingletonPage.xaml?ordinal={0}", ordinal), UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // set the context...
            this.DataContext = new ConfigurePageViewModel();
        }

        private void listBookmarks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bookmark selected = (Bookmark)this.listBookmarks.SelectedItem;
            if (selected != null)
                ConfigureBookmark(selected.Ordinal);
        }
    }
}