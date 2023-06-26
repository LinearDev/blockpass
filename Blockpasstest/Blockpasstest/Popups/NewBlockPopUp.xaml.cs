using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Blockpasstest.Custom;

namespace Blockpasstest.Popups
{	
	public partial class NewBlockPopUp : PopupPage
	{
        public Color GroupColor
        {
            get;
            set;
        }

		public NewBlockPopUp ()
		{
			InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ColorHexText.Text = "#FF0000";
        }

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();

            myPopupTranslation.TranslationY = App.Current.MainPage.Height;
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

        void PickedColorChanged(System.Object sender, Xamarin.Forms.Color e)
        {
            GroupColor = e;
            ShowColor.BackgroundColor = e;
            string hexColor = e.ToHex();
            hexColor = "#" + hexColor.Substring(3);
            ColorHexText.Text = hexColor;
        }

        async void CreateNewGroup(System.Object sender, System.EventArgs e)
        {
            WebAssemblyRuntime ice = new WebAssemblyRuntime();
            Custom.RPC rpc = new Custom.RPC();
            string hexColor = GroupColor.ToHex();
            hexColor = "#" + hexColor.Substring(3);
            hexColor = ice.Encode(hexColor);
            string group = ice.Encode(GroupNameEntry.Text);

            await rpc.addNewGroup(group, hexColor);
            await rpc.addActivityRecord("Created new Block");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}

