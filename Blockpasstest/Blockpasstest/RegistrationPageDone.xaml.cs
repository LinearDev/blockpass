using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Blockpasstest.Custom;
using Xamarin.Essentials;
using Blockpasstest.icons;

namespace Blockpasstest
{	
	public partial class RegistrationPageDone : ContentPage
	{	
		public RegistrationPageDone()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		public async void GoToLogin(System.Object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new LoginPage(), true);
		}
    }
}

