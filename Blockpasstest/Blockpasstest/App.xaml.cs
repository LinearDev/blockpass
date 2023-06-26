using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Blockpasstest.Custom;

namespace Blockpasstest
{
    public partial class App : Application
    {
        public static Custom.Store Storage { get; private set; }

        public App ()
        {
            Storage = new Custom.Store();
            InitializeComponent();
        }

        protected override void OnStart ()
        {
            var mainPage = new MainPage();
            var navigationPage = new NavigationPage(mainPage);
            if (Preferences.Get("FaceId", "false") == "true")
            {
                //AuthenticateWithFaceIDAsync();
            }
            MainPage = navigationPage;
        }

        protected override void OnSleep ()
        {
        }

        private async void CheckOnResume()
        {
            if (string.IsNullOrEmpty(Preferences.Get("__account", null)))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage(), true);
            }
            else
            {
                if (string.IsNullOrEmpty(Preferences.Get("Code", null)))
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage(), true);
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new PassCodePage(), true);
                }
            }
        }

        protected override void OnResume ()
        {
            
        }
    }
}

