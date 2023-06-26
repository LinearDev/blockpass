using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Blockpasstest.Custom;

namespace Blockpasstest
{
	public partial class PassCodePage : ContentPage
	{
        private Frame[] inputControll;
        private string Code = "";
        private bool firstCode = false;

        public PassCodePage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
            inputControll = new Frame[] {DotOne, DotTwo, DotThree, DotFour};
            string storedCode = Preferences.Get("Code", "");
            if (storedCode == "") { firstCode = true; }
        }

        private async void UpdateDots()
        {
            for (int i = 0; i < inputControll.Length; i++)
            {
                if (Code.Length > i)
                {
                    inputControll[i].Background = new SolidColorBrush(Color.FromHex("#9E9E9E"));
                    inputControll[i].ForceLayout();
                } else
                {
                    inputControll[i].Background = new SolidColorBrush(Color.FromHex("#F4F4F4"));
                    inputControll[i].ForceLayout();
                }
            }
            
            if (Code.Length == 4)
            {
                var rpc = new Custom.RPC();
                if (firstCode)
                {
                    Preferences.Set("Code", Code);
                    rpc.addActivityRecord("A code to log in to the application was set");
                    await Navigation.PushAsync(new PassCodePage(), true);
                    return;
                }
                var storedCode = Preferences.Get("Code", "");
                if (storedCode == Code)
                {
                    await App.Storage.InitialLoad();
                    rpc.addActivityRecord("Logging in to the app");
                    await Navigation.PushAsync(new HomePage(), true);
                } else
                {
                    Code = "";
                    UpdateDots();
                }
            }
        }

        void Button_One(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "1";
                UpdateDots();
            }
        }

        void Button_Two(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "2";
                UpdateDots();
            }
        }

        void Button_Three(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "3";
                UpdateDots();
            }
        }

        void Button_Four(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "4";
                UpdateDots();
            }
        }

        void Button_Five(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "5";
                UpdateDots();
            }
        }

        void Button_Six(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "6";
                UpdateDots();
            }
        }

        void Button_Seven(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "7";
                UpdateDots();
            }
        }

        void Button_Eight(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "8";
                UpdateDots();
            }
        }

        void Button_Nine(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "9";
                UpdateDots();
            }
        }

        void Button_Zero(System.Object sender, System.EventArgs e)
        {
            if (Code.Length != 4)
            {
                Code += "0";
                UpdateDots();
            }
        }

        void Button_DeleteOne(System.Object sender, System.EventArgs e)
        {
            if (Code.Length > 0)
            {
                OnAnimateOpacityClicked(sender, e);
                Code = Code.Substring(0, Code.Length - 1);
                UpdateDots();
            }
        }

        private async Task OnAnimateOpacityClicked(object sender, EventArgs e)
        {
            var animation = new Animation();

            var fadeOutAnimation = new Animation(v => DeleteBtn.Opacity = v, 1, 0.8);
            var fadeInAnimation = new Animation(v => DeleteBtn.Opacity = v, 0.8, 1);

            animation.Add(0, 0.5, fadeOutAnimation);
            animation.Add(0.5, 1, fadeInAnimation);

            animation.Commit(DeleteBtn, "OpacityAnimation", length: 1000, easing: Easing.Linear);

            await Task.Delay(3000); // Wait for 3 seconds

            ViewExtensions.CancelAnimations(DeleteBtn); // Stop the animation
        }
    }
}

