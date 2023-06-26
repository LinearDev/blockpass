using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Blockpasstest.Custom;
using Xamarin.Essentials;
using Blockpasstest.icons;
using System.Threading.Tasks;
using Blockpasstest.Home;

namespace Blockpasstest
{
    public partial class HomePage : ContentPage
    {
        private string CurrentPage = "Home";

        public HomePage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MainContent.Content = new HomeContent();
        }

        private void UpdateMenu()
        {
            Color colorActive = Color.FromHex("#5492F9");
            Color color = Color.FromHex("#000");
            HomeBtn.CustomColor = color;
            BlocksBtn.CustomColor = color;
            SearchBtn.CustomColor = color;
            ProfileBtn.CustomColor = color;
            switch (CurrentPage)
            {
                case "Home":
                    HomeBtn.CustomColor = colorActive;
                    break;
                case "Blocks":
                    BlocksBtn.CustomColor = colorActive;
                    break;
                case "Search":
                    SearchBtn.CustomColor = colorActive;
                    break;
                case "Profile":
                    ProfileBtn.CustomColor = colorActive;
                    break;
                default:
                    break;
            }
        }

        void GoToHome(System.Object sender, System.EventArgs e)
        {
            if (CurrentPage != "Home")
            {
                MainContent.Content = new HomeContent();
                CurrentPage = "Home";
                UpdateMenu();
            }
        }

        void GoToBlocks(System.Object sender, System.EventArgs e)
        {
            if (CurrentPage != "Blocks")
            {
                MainContent.Content = new BlocksContent();
                CurrentPage = "Blocks";
                UpdateMenu();
            }
        }

        void GoToSearch(System.Object sender, System.EventArgs e)
        {
            if (CurrentPage != "Search")
            {
                MainContent.Content = new SearchContent();
                CurrentPage = "Search";
                UpdateMenu();
            }
        }

        void GoToProfile(System.Object sender, System.EventArgs e)
        {
            if (CurrentPage != "Profile")
            {
                MainContent.Content = new ProfileContent();
                CurrentPage = "Profile";
                UpdateMenu();
            }
        }
    }
}

