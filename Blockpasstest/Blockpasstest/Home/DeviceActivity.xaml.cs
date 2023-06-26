using System;
using System.Collections.Generic;
using Blockpasstest.Home.DeviceActivityContext;

using Xamarin.Forms;

namespace Blockpasstest.Home
{	
	public partial class DeviceActivity : ContentPage
	{
        List<List<Custom.RPC.UserActivity>> activities = new List<List<Custom.RPC.UserActivity>>();

		public DeviceActivity ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            this.InitLoading();
        }

        async void InitLoading()
        {
            var rpc = new Custom.RPC();
            (List < Custom.RPC.UserActivity > data, Custom.RPC.ActivityLength lenght) = await rpc.getUserActivity();
            if (data.Count > 0)
            {
                string current = "";
                int i = 0;
                foreach (Custom.RPC.UserActivity act in data)
                {
                    long.TryParse(act.Time, out long unixTimestamp);
                    string dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).ToOffset(DateTimeOffset.Now.Offset).ToString("dd MMM yyyy");
                    if (dateTime != current) {
                        if (current != "") { i++; }
                        current = dateTime;
                        List<Custom.RPC.UserActivity> newAct = new List<Custom.RPC.UserActivity> { act };
                        activities.Add(newAct);
                    } else
                    {
                        activities[i].Add(act);
                    }
                }

                ActivityData.Children.Clear();

                activities.Reverse();
                foreach (List<Custom.RPC.UserActivity> a in activities)
                {
                    ActivityForm form = new ActivityForm();
                    a.Reverse();
                    form.Activities = a;
                    form.Load();
                    ActivityData.Children.Add(form);
                }
            }
        }

        async void BackToProfile(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

