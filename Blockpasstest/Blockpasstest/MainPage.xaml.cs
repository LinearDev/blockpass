using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Blockpasstest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.ToPage();
        }

        public async void ToPage()
        {
            await Task.Delay(2000);
            if (string.IsNullOrEmpty(Preferences.Get("__account", null)))
            {
                await Navigation.PushAsync(new LoginPage(), true);
            }
            else
            {
                if (string.IsNullOrEmpty(Preferences.Get("Code", null)))
                {
                    await Navigation.PushAsync(new LoginPage(), true);
                }
                else
                {
                    await Navigation.PushAsync(new PassCodePage(), true);
                }
            }
        }
    }
}

