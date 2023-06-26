using System;
using System.Collections.Generic;
using Blockpasstest.icons;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace Blockpasstest.Home
{	
	public partial class GroupComponent : ContentView
	{
        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public static BindableProperty GroupNameProperty =
            BindableProperty.Create(nameof(GroupName), typeof(string), typeof(GroupComponent), "");

        public Color GroupColor
        {
            get => (Color)GetValue(GroupColorProperty);
            set => SetValue(GroupColorProperty, value);
        }

        public static readonly BindableProperty GroupColorProperty =
            BindableProperty.Create(nameof(GroupColor), typeof(Color), typeof(GroupComponent), Color.Red);

        public GroupComponent ()
		{
			InitializeComponent();
		}
    }
}

