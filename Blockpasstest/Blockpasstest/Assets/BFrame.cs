using System;
using Xamarin.Forms;

namespace Blockpasstest.Assets
{
	public class BFrame : Frame
	{
        public static readonly BindableProperty ShadowColorProperty =
        BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(BFrame), Color.Default);

        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        public static readonly BindableProperty ShadowOpacityProperty =
            BindableProperty.Create(nameof(ShadowOpacity), typeof(double), typeof(BFrame), 1.0);

        public double ShadowOpacity
        {
            get { return (double)GetValue(ShadowOpacityProperty); }
            set { SetValue(ShadowOpacityProperty, value); }
        }

        public static readonly BindableProperty ShadowRadiusProperty =
            BindableProperty.Create(nameof(ShadowRadius), typeof(double), typeof(BFrame), 5.0);

        public double ShadowRadius
        {
            get { return (double)GetValue(ShadowRadiusProperty); }
            set { SetValue(ShadowRadiusProperty, value); }
        }

        public BFrame()
        {
            SetDynamicResource(BackgroundColorProperty, "CustomFrameBackgroundColor");
            SetDynamicResource(HasShadowProperty, "CustomFrameHasShadow");

            SetBinding(CornerRadiusProperty, new Binding(nameof(CornerRadius), source: this));
        }
    }
}

