using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Blockpasstest.Custom;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms.PancakeView;
using Blockpasstest.icons;
using System.Linq;

namespace Blockpasstest.Popups
{	
	public partial class NewBlockpassPopUp : PopupPage
	{
        private int fieldsOnPage = 2;
        private List<Custom.RPC.Group> loadedGroups { get; set; }

		public NewBlockpassPopUp()
		{
			InitializeComponent();
        }

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
            Task.Run(LoadGroups);
            myPopupTranslation.TranslationY = App.Current.MainPage.Height;
        }

        private async Task LoadGroups()
        {
            List<Custom.RPC.Group> groups = App.Storage.useStoreState<List<Custom.RPC.Group>>("groups", "groups");
            this.loadedGroups = groups;
            var options = new List<string> { };
            foreach (Custom.RPC.Group group in groups )
            {
                options.Add(group.Title);
            }
            GroupsInput.ItemsSource = options;
        }

        protected override async Task OnAppearingAnimationEndAsync()
        {
            await Task.WhenAll(
                myPopupTranslation.TranslateTo(0, 0, 250, Easing.SinOut)
            );
        }

        protected override async Task OnDisappearingAnimationBeginAsync()
        {
            await Task.WhenAll(
                myPopupTranslation.TranslateTo(0, App.Current.MainPage.Height, 250, Easing.SinIn)
            );
        }

        async void ClosePopUp(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void CreateField(string FieldName)
        {
            StackLayout stackLayout = new StackLayout
            {
                AutomationId = FieldName,
                Margin = new Thickness(0, 0, 0, 14)
            };

            Grid titleGrid = new Grid();
            titleGrid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Star }
            );

            Label field2Label = new Label
            {
                Text = FieldName,
                FontFamily = "SourceSansPro-SemiBold.ttf",
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 6),
                TextColor = Color.FromHex("#9e9e9e")
            };

            titleGrid.Children.Add(field2Label);

            PancakeView deleteBtn = new PancakeView();

            Grid.SetColumn(deleteBtn, 0);
            deleteBtn.HorizontalOptions = LayoutOptions.End;

            TapGestureRecognizer deleteBtnGesture = new TapGestureRecognizer();
            deleteBtnGesture.Tapped += DeleteFields;
            deleteBtn.GestureRecognizers.Add(deleteBtnGesture);

            TrashPopupSVG trashPopupSVG = new TrashPopupSVG
            {
                Margin = new Thickness(0, 0, 5, 0),
                IsVisible = true,
                IsEnabled = false,
                IsTabStop = false,
                InputTransparent = true,
                VerticalOptions = LayoutOptions.Start
            };

            deleteBtn.Content = trashPopupSVG;
            titleGrid.Children.Add(deleteBtn);

            Frame frame1 = new Frame
            {
                HasShadow = false,
                Padding = new Thickness(0, 14),
                CornerRadius = 10,
                BackgroundColor = Color.FromHex("#f4f4f4")
            };

            AdvancedEntry entry1 = new AdvancedEntry
            {
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                CornerRadius = 0,
                Placeholder = "Password",
                PlaceholderColor = Color.FromHex("#9e9e9e"),
                FontFamily = "Montserrat-Regular.ttf",
                TextFontSize = 14,
                Margin = new Thickness(-25, 0, 0, 0),
                TextColor = Color.Black
            };

            Frame frame2 = new Frame
            {
                HasShadow = false,
                Padding = new Thickness(0, 14),
                CornerRadius = 10,
                BackgroundColor = Color.FromHex("#f4f4f4")
            };

            AdvancedEntry entry2 = new AdvancedEntry
            {
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                CornerRadius = 0,
                Placeholder = "MySavePassword",
                PlaceholderColor = Color.FromHex("#9e9e9e"),
                FontFamily = "Montserrat-Regular.ttf",
                TextFontSize = 14,
                Margin = new Thickness(-25, 0, 0, 0),
                TextColor = Color.Black
            };

            frame1.Content = entry1;
            frame2.Content = entry2;
            stackLayout.Children.Add(titleGrid);
            stackLayout.Children.Add(frame1);
            stackLayout.Children.Add(frame2);
            FieldsContext.Children.Add(stackLayout);
        }

        void AddNewField(System.Object sender, System.EventArgs e)
        {
            fieldsOnPage += 1;
            CreateField("Field "+ fieldsOnPage.ToString());
        }

        void DeleteFields(System.Object sender, System.EventArgs e)
        {
            PancakeView deleteBtn = (PancakeView)sender;
            StackLayout stackLayout = deleteBtn.Parent.Parent as StackLayout; // Получить родительский StackLayout

            if (stackLayout != null)
            {
                FieldsContext.Children.Remove(stackLayout);
                fieldsOnPage -= 1;

                int startIndex = 3;

                foreach (var child in FieldsContext.Children)
                {
                    if (child is StackLayout stack)
                    {
                        foreach (var element in stack.Children)
                        {
                            if (element is Grid grid)
                            {
                                foreach (var item in grid.Children)
                                {
                                    if (item is Label label)
                                    {
                                        label.Text = "Field " + startIndex.ToString();
                                        startIndex += 1;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        async void CreateNewPassword(System.Object sender, System.EventArgs e)
        {
            WebAssemblyRuntime ice = new WebAssemblyRuntime();
            string title = GroupNameEntry.Text;
            if (string.IsNullOrEmpty(title)) { return; }
            string groupName = GroupsInput.SelectedItem.ToString();
            string groupId = "0";

            //select group
            foreach (var group in this.loadedGroups)
            {
                if (groupName == group.Title)
                {
                    groupId = (group.Id + 1).ToString();
                }
            }

            //getting fields
            List<List<string>> dataMid = new List<List<string>>();
            for (int f = 0; f < this.fieldsOnPage; f++)
            {
                dataMid.Add(new List<string>());
            }

            int i = 0;
            int l = 0;

            foreach (var child in FieldsContext.Children)
            {
                if (child is StackLayout stack)
                {
                    foreach (var element in stack.Children)
                    {

                        if (element is Frame fr)
                        {
                            if (i != 0)
                            {
                                string text = (fr.Content as AdvancedEntry).Text;
                                dataMid[l].Add(text);
                                if (i % 2 == 0) { l++; }
                            }
                            i++;
                        }
                    }
                }
            }

            bool oneOrMoreIsNull = false;

            List<RPC.PassData> passData = new List<RPC.PassData>();
            foreach (List<string> grouped in dataMid)
            {
                if (string.IsNullOrEmpty(grouped[0]) || string.IsNullOrEmpty(grouped[1])) { oneOrMoreIsNull = true; }
                passData.Add(new RPC.PassData()
                {
                    Data = ice.Encode(grouped[0]),
                    Title = ice.Encode(grouped[1])
                });
            }

            if (oneOrMoreIsNull) { return; }

            Custom.RPC rpc = new Custom.RPC();
            title = ice.Encode(title);
            await rpc.addNewPassword(title, groupId, passData);
            await rpc.addActivityRecord("Created new Blockpass");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}

