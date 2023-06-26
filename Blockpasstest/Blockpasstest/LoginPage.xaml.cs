using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Blockpasstest.Custom;
using Xamarin.Essentials;
using Blockpasstest.icons;
using System.Threading.Tasks;
using Xamarin.Forms.PancakeView;

namespace Blockpasstest
{	
	public partial class LoginPage : ContentPage
	{	
		public LoginPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private int clicks = 0;

        async void CreatePublic(System.Object sender, System.EventArgs e)
        {
            OnAnimateOpacityClicked(sender, e);

            if (!string.IsNullOrEmpty(PhraseInput?.Text))
            {
                Custom.Account.User account = new Custom.Account().CreateSpesificAccount(PhraseInput.Text);
                Preferences.Set("__account", account.address);
                Preferences.Set("__accountPublic", account.publicKey);
                Preferences.Set("__accountPrivate", account.privateKey);
                var rpc = new Custom.RPC();
                rpc.addActivityRecord("New account login");
                await Navigation.PushAsync(new PassCodePage(), true);
            }
        }

        async void GoToRegistration(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage(), true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var rotateAnimation = new Animation(v => LogoBlueSVG.Rotation = v, 360, 0);
            rotateAnimation.Commit(LogoBlueSVG, "RotationAnimation", length: 5000, easing: Easing.Linear, repeat: () => true);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewExtensions.CancelAnimations(LogoBlueSVG);
        }

        private async Task OnAnimateOpacityClicked(object sender, EventArgs e)
        {
            var animation = new Animation();

            var fadeOutAnimation = new Animation(v => CreateAccountBtn.Opacity = v, 1, 0.8);
            var fadeInAnimation = new Animation(v => CreateAccountBtn.Opacity = v, 0.8, 1);

            animation.Add(0, 0.5, fadeOutAnimation);
            animation.Add(0.5, 1, fadeInAnimation);

            animation.Commit(CreateAccountBtn, "OpacityAnimation", length: 1000, easing: Easing.Linear);

            await Task.Delay(3000); // Wait for 3 seconds

            ViewExtensions.CancelAnimations(CreateAccountBtn); // Stop the animation
        }
    }
}

