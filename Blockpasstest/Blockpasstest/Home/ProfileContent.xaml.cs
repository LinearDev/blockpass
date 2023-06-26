using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Blockpasstest.Home
{	
	public partial class ProfileContent : ContentView
	{	
		public ProfileContent ()
		{
			InitializeComponent ();

			LoadContent();
		}

		private void LoadContent()
		{
			string address = Preferences.Get("__account", "");
			if (address.Length != 0)
			{
				Address.Text = address.Substring(0, 7) + "..." + address.Substring(address.Length - 8);
			}
		}

        async void CopyAddress(System.Object sender, System.EventArgs e)
        {
			string address = Preferences.Get("__account", "");
			await Clipboard.SetTextAsync(address);
			var rpc = new Custom.RPC();
			rpc.addActivityRecord("The public address of the account was copied");
        }

        void GoToActivity(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new DeviceActivity());
        }

        async void LogOut(System.Object sender, System.EventArgs e)
        {
            var rpc = new Custom.RPC();
            await rpc.addActivityRecord("The public address of the account was copied");
            Preferences.Clear();
			await Navigation.PushAsync(new MainPage());
        }
    }
}

