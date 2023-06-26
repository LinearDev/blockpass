using System;
using System.Collections.Generic;
using Xamarin.Forms;

using Blockpasstest.icons;

namespace Blockpasstest.Home
{	
	public partial class PassComponent : ContentView
	{
		public class DataType
		{
			public string name;
			public string data;
		}
		public bool viewMode = false;

		public BindableProperty CustomTitleProperty =
			BindableProperty.Create(nameof(CustomTitle), typeof(string), typeof(PassComponent), "");

		public string CustomTitle
		{
			set => SetValue(CustomTitleProperty, value);
			get => (string)GetValue(CustomTitleProperty);
		}

		public BindableProperty CustomDataProperty =
			BindableProperty.Create(nameof(CustomData), typeof(List<DataType>), typeof(PassComponent), new List<DataType>(), propertyChanged: OnCustomDataPropertyChanged);

		public List<DataType> CustomData
		{
			get => (List<DataType>)GetValue(CustomDataProperty);
			set => SetValue(CustomDataProperty, value);
		}

		public PassComponent ()
		{
			InitializeComponent ();
            UpdateContent();
        }

        private static void OnCustomDataPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PassComponent passComponent)
            {
                passComponent.UpdateContent();
            }
        }

        public void UpdateContent()
        {
            var ice = new Custom.WebAssemblyRuntime();
            foreach (DataType data in CustomData)
            {
                Grid grid = new Grid
                {
                    Padding = 0,
                    BackgroundColor = Color.Transparent
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                Label label1 = new Label
                {
                    Text = ice.Decode(data.name) + ": ",
                    FontFamily = "SourceSansPro-Regular.ttf",
                    FontSize = 14,
                };

                Label label2 = new Label
                {
                    Text = "••••••••••",
                    FontFamily = "SourceSansPro-Regular.ttf",
                    FontSize = 14
                };

                Grid.SetColumn(label1, 0);
                Grid.SetColumn(label2, 1);

                grid.Children.Add(label1);
                grid.Children.Add(label2);
                LabelContext.Children.Add(grid);
            }
        }

        void SwitchViewMode(System.Object sender, System.EventArgs e)
        {
            EyeCloseSVG EyeClose = new EyeCloseSVG();
            EyeClose.IsVisible = true;
            EyeClose.InputTransparent = true;
            EyeClose.IsEnabled = false;

            EyeOpenSVG EyeOpen = new EyeOpenSVG();
            EyeOpen.IsVisible = true;
            EyeOpen.InputTransparent = true;
            EyeOpen.IsEnabled = false;
            EyeOpen.Padding = new Thickness(0, 0, 0, 2);

            if (!viewMode)
            {
                var rpc = new Custom.RPC();
                rpc.addActivityRecord("One of the Blockpasses was viewed");
                ViewIcon.Content = EyeClose;
                ShowContext();
            } else
            {
                LabelContext.Children.Clear();
                UpdateContent();
                ViewIcon.Content = EyeOpen;
            }

            viewMode = !viewMode;
        }

        private void ShowContext()
        {
            LabelContext.Children.Clear();

            var ice = new Custom.WebAssemblyRuntime();
            foreach (DataType data in CustomData)
            {
                Grid grid = new Grid
                {
                    Padding = 0,
                    BackgroundColor = Color.Transparent
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                Label label1 = new Label
                {
                    Text = ice.Decode(data.name) + ": ",
                    FontFamily = "SourceSansPro-Regular.ttf",
                    FontSize = 14,
                };

                Label label2 = new Label
                {
                    Text = ice.Decode(data.data),
                    FontFamily = "SourceSansPro-Regular.ttf",
                    FontSize = 14
                };

                Grid.SetColumn(label1, 0);
                Grid.SetColumn(label2, 1);

                grid.Children.Add(label1);
                grid.Children.Add(label2);
                LabelContext.Children.Add(grid);
            }
        }
    }
}

