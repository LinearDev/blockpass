using System;
using System.Collections.Generic;
using Blockpasstest.icons;
using Xamarin.Forms;

namespace Blockpasstest.Home
{	
	public partial class HeadingComponent : ContentView
	{
        public static readonly BindableProperty CustomTitleProperty =
            BindableProperty.Create(nameof(CustomTitle), typeof(string), typeof(HeadingComponent), "");

        public string CustomTitle
        {
            get => (string)GetValue(CustomTitleProperty);
            set => SetValue(CustomTitleProperty, value);
        }

        public static readonly BindableProperty CustomSubTitleProperty =
            BindableProperty.Create(nameof(CustomSubTitle), typeof(string), typeof(HeadingComponent), "");

        public string CustomSubTitle
        {
            get => (string)GetValue(CustomSubTitleProperty);
            set => SetValue(CustomSubTitleProperty, value);
        }

        public HeadingComponent ()
		{
			InitializeComponent ();
		}
	}
}

