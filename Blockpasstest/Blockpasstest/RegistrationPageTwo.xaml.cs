using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Blockpasstest.Custom;
using Xamarin.Essentials;
using Blockpasstest.icons;

namespace Blockpasstest
{	
	public partial class RegistrationPageTwo : ContentPage
	{	
		public RegistrationPageTwo()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var rotateAnimation = new Animation(v => LogoBlueSVG.Rotation = v, 0, 360);
            rotateAnimation.Commit(LogoBlueSVG, "RotationAnimation", length: 4000, easing: Easing.Linear, repeat: () => true);

            Device.BeginInvokeOnMainThread(() =>
            {
                var account = Preferences.Get("__accountPrivate", null);
                if (account == null) { return; }
                PhraseInput.Text = account;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewExtensions.CancelAnimations(LogoBlueSVG);
        }

        void CopyAddress(System.Object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(PhraseInput?.Text))
            {
                Clipboard.SetTextAsync(PhraseInput.Text);
            }
        }

        async void GoToDonePage(System.Object sender, System.EventArgs e)
        {
            var rpc = new Custom.RPC();
            await rpc.addActivityRecord("Account was created");
            Preferences.Remove("__accountPrivate");
            await Navigation.PushAsync(new RegistrationPageDone(), true);
        }
    }
}

