using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Blockpasstest.Popups;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Blockpasstest.Custom;
using Blockpasstest.icons;

namespace Blockpasstest.Home
{
	public partial class BlocksContent : ContentView
	{
		public class Group
		{
			public string name;
			public string color;
		}

		List<Custom.RPC.Group> groups { get; set; }

        public BlocksContent ()
		{
			this.groups = App.Storage.useStoreState<List<Blockpasstest.Custom.RPC.Group>>("groups", "groups");
            InitializeComponent ();
            InitialLoader();
        }

		public void InitialLoader()
		{
            App.Storage.Subscribe("groups", storedGroups =>
			{
                var loading = storedGroups.GetType().GetProperty("loading")?.GetValue(storedGroups);
                var groupsNew = storedGroups.GetType().GetProperty("groups")?.GetValue(storedGroups);

                if (loading != null && loading is bool loadingValue)
                {
                    //
                }

                if (groupsNew != null && groupsNew is List<Blockpasstest.Custom.RPC.Group> groupList)
                {
					this.groups = groupList;
					this.GetGroups();
                }
            });

			GetGroups();
        }

        public void GetGroups()
		{
            GroupsContext.Children.Clear();

			if (groups.Count == 0)
			{
				Label empty = new Label();
				empty.Text = "You don't have Blocks yet.\nCreate a new one!";
				empty.Margin = new Thickness(0, 25, 0, 0);
				empty.FontFamily = "SourceSansPro-SemiBold.ttf";
				empty.FontSize = 16;
				empty.TextColor = Color.FromHex("#9e9e9e");
				empty.HorizontalTextAlignment = TextAlignment.Center;

				GroupsContext.Children.Add(empty);
				return;
			}
			WebAssemblyRuntime ice = new WebAssemblyRuntime();
			for (int i = 0; i < groups.Count; i++)
			{
				var component = new GroupComponent();
				component.GroupName = ice.Decode(groups[i].Title);
				component.GroupColor = Color.FromHex(ice.Decode(groups[i].Color));

				GroupsContext.Children.Add(component);
			}
		}

        async void NewBlock(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewBlockPopUp());
        }
    }
}

