using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace Blockpasstest.icons
{	
	public partial class SearchBtnSVG : ContentView
	{	
		public SearchBtnSVG ()
		{
			InitializeComponent ();
		}

        public static readonly BindableProperty CustomColorProperty =
            BindableProperty.Create(nameof(CustomColor), typeof(Color), typeof(HomeBtnSVG), Color.Red);

        public Color CustomColor
        {
            get => (Color)GetValue(CustomColorProperty);
            set => SetValue(CustomColorProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            Corner.Stroke = new SolidColorBrush(CustomColor);
            Main.Stroke = new SolidColorBrush(CustomColor);
        }
    }
}

