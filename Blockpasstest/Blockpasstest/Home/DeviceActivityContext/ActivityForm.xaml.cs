using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Blockpasstest.Home.DeviceActivityContext
{	
	public partial class ActivityForm : ContentView
	{
        public List<Custom.RPC.UserActivity> Activities { get; set; }

		public ActivityForm ()
		{
			InitializeComponent();
            
        }

        public void Load()
        {
            if (this.Activities.Count > 0)
            {
                var format = new Custom.Formatter();
                Time.Text = format.TimeFormatter(this.Activities[0].Time);

                foreach (Custom.RPC.UserActivity act in this.Activities)
                {
                    this.CreateRecord(act.Title, act.DeviceType, act.Time);
                }
            }
        }

		public void CreateRecord(string title, string device, string time)
		{
            var stackLayout = new StackLayout();

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var loginLabel = new Label
            {
                Text = title,
                FontFamily = "SourceSansPro-SemiBold.ttf",
                FontSize = 16
            };
            Grid.SetColumn(loginLabel, 0);

            long.TryParse(time, out long dateTime);
            string t = DateTimeOffset.FromUnixTimeSeconds(dateTime).ToOffset(DateTimeOffset.Now.Offset).ToString("HH:mm");

            var timeLabel = new Label
            {
                Text = t,
                FontFamily = "SourceSansPro-Regular.ttf",
                FontSize = 16
            };
            Grid.SetColumn(timeLabel, 1);

            grid.Children.Add(loginLabel);
            grid.Children.Add(timeLabel);

            var deviceLabel = new Label
            {
                Text = "Device: " + device,
                FontFamily = "SourceSansPro-Regular.ttf",
                TextColor = Color.FromHex("#9e9e9e"),
                FontSize = 14
            };

            var frame = new Frame
            {
                HasShadow = false,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                BackgroundColor = Color.FromHex("#f4f4f4"),
                HeightRequest = 2
            };

            stackLayout.Children.Add(grid);
            stackLayout.Children.Add(deviceLabel);
            stackLayout.Children.Add(frame);
            Content.Children.Add(stackLayout);
        }
	}
}

