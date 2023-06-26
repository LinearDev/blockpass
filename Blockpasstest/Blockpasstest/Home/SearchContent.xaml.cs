using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Blockpasstest.Home
{	
	public partial class SearchContent : ContentView
	{
        List<Custom.RPC.Password> passwords { get; set; }

		public SearchContent ()
		{
			InitializeComponent();
            Init();
		}

        private void CreateNull()
        {
            SearchContentBox.Children.Clear();

            Label noResults = new Label()
            {
                Text = "Here will be your search results",
                FontFamily = "SourceSansPro-SemiBold.ttf",
                FontSize = 14,
                TextColor = Color.FromHex("#000"),
                Margin = new Thickness(0, 20, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            SearchContentBox.Children.Add(noResults);
        }

        private void Init()
        {
            passwords = App.Storage.useStoreState<List<Custom.RPC.Password>>("passwords", "passwords");

            App.Storage.Subscribe("passwords", (passwords) =>
            {
                var passwordsNew = passwords.GetType().GetProperty("passwords")?.GetValue(passwords);

                if (passwordsNew != null && passwordsNew is List<Blockpasstest.Custom.RPC.Password> passwordsList)
                {
                    this.passwords = passwordsList;
                }
            });

            this.CreateNull();
        }

        void Search_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            EntryPlaceholder.HorizontalOptions = LayoutOptions.Start;
            EntryPlaceholderText.Text = "";
        }

        void Search_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchEntry.Text))
            {
                EntryPlaceholder.HorizontalOptions = LayoutOptions.Center;
                EntryPlaceholderText.Text = "Search";
            }
        }

        void SearchEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var ice = new Custom.WebAssemblyRuntime();
            string search = e.NewTextValue;
            if (!string.IsNullOrEmpty(search))
            {
                SearchContentBox.Children.Clear();

                foreach (var pass in this.passwords)
                {
                    string dec = ice.Decode(pass.Title);
                    if (dec.ToLowerInvariant().Contains(search.ToLowerInvariant()))
                    {
                        var passCont = new PassComponent();
                        passCont.CustomTitle = dec;

                        for (int a = 0; a < pass.Data.Count; a++)
                        {
                            passCont.CustomData.Add(new PassComponent.DataType
                            {
                                name = pass.Data[a].Data,
                                data = pass.Data[a].Title
                            });
                        }

                        passCont.UpdateContent();
                        SearchContentBox.Children.Add(passCont);
                    }
                }
            }
            else
            {
                this.CreateNull();
            }
        }
    }
}

