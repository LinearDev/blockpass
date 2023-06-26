using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace Blockpasstest.icons
{	
	public partial class HomeBtnSVG : ContentView
	{
        public static readonly BindableProperty CustomColorProperty =
            BindableProperty.Create(nameof(CustomColor), typeof(Color), typeof(HomeBtnSVG), Color.Red);

        public Color CustomColor
        {
            get => (Color)GetValue(CustomColorProperty);
            set => SetValue(CustomColorProperty, value);
        }

        public HomeBtnSVG()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            Corner.Stroke = new SolidColorBrush(CustomColor);
            Main.Stroke = new SolidColorBrush(CustomColor);
        }
    }

    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return new SolidColorBrush(color);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

