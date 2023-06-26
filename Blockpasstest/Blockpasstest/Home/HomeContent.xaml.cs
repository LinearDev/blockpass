using System;
using System.Collections.Generic;
using Blockpasstest.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Blockpasstest.Custom;
using Xamarin.Forms.Internals;

namespace Blockpasstest.Home
{	
	public partial class HomeContent : ContentView
	{
		private List<RPC.Password> passList { get; set; }

		public HomeContent ()
		{
			this.passList = App.Storage.useStoreState<List<RPC.Password>>("passwords", "passwords");
			InitializeComponent ();
			CreateContext();
            InitialLoader();
        }

        public void InitialLoader()
        {
            App.Storage.Subscribe("passwords", storedPasswords =>
            {
                var loading = storedPasswords.GetType().GetProperty("loading")?.GetValue(storedPasswords);
                var passwordsNew = storedPasswords.GetType().GetProperty("passwords")?.GetValue(storedPasswords);

                if (loading != null && loading is bool loadingValue)
                {
                    //
                }

                if (passwordsNew != null && passwordsNew is List<Blockpasstest.Custom.RPC.Password> passwordsList)
                {
                    this.passList = passwordsList;
                    this.CreateContext();
                }
            });

            CreateContext();
        }

        private void CreateContext()
		{
            PasswordsContext.Children.Clear();

            if (passList.Count == 0)
            {
                Label empty = new Label();
                empty.Text = "You don't have Passwords yet.\nCreate a new one!";
                empty.Margin = new Thickness(0, 25, 0, 0);
                empty.FontFamily = "SourceSansPro-SemiBold.ttf";
                empty.FontSize = 16;
                empty.TextColor = Color.FromHex("#9e9e9e");
                empty.HorizontalTextAlignment = TextAlignment.Center;

                PasswordsContext.Children.Add(empty);
                return;
            }

            var ice = new WebAssemblyRuntime();
            for (int i = 0; i < passList.Count; i++)
            {
                PassComponent passComponent = new PassComponent();
                passComponent.CustomTitle = ice.Decode(passList[i].Title);

                for (int a = 0; a < passList[i].Data.Count; a++)
                {
                    passComponent.CustomData.Add(new PassComponent.DataType
                    {
                        name = passList[i].Data[a].Data,
                        data = passList[i].Data[a].Title
                    });
                }
                PasswordsContext.Children.Add(passComponent);
                passComponent.UpdateContent();
            }
        }

        async void CreateNewBlockpass(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewBlockpassPopUp());
        }
    }
}

