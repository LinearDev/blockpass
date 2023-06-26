using System;

using Android.App;
using Blockpasstest.Assets;
using Blockpasstest.Droid;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Text;

[assembly: ExportRenderer(typeof(BorderedEntry), typeof(BorderedEntryRenderer))]
[assembly: ExportRenderer(typeof(BFrame), typeof(BFrameRenderer))]
namespace Blockpasstest.Droid
{
    [Activity(Label = "Blockpasstest", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class BorderedEntryRenderer : EntryRenderer
    {
        public BorderedEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                var view = (BorderedEntry)Element;
                var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                shape.Paint.Color = view.BorderColor.ToAndroid();
                shape.Paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
                shape.Paint.StrokeWidth = view.BorderWidth;
                Control.Background = shape;

                Control.SetPadding(
                    Control.PaddingLeft,
                    Control.PaddingTop,
                    Control.PaddingRight,
                    Control.PaddingBottom);
                Control.SetBackground(shape);
                Control.SetHintTextColor(view.PlaceholderColor.ToAndroid());
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsFocused" && Control != null && Element != null)
            {
                if (Element.IsFocused)
                {
                    Control.RequestFocus();
                }
                else
                {
                    Control.ClearFocus();
                }
            }
        }
    }

    public class BFrameRenderer : FrameRenderer
    {
        public BFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                UpdateShadow();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == BFrame.ShadowColorProperty.PropertyName ||
                e.PropertyName == BFrame.ShadowOpacityProperty.PropertyName ||
                e.PropertyName == BFrame.ShadowRadiusProperty.PropertyName)
            {
                UpdateShadow();
            }
        }

        private void UpdateShadow()
        {
            var customFrame = Element as BFrame;
            if (customFrame != null)
            {
                var shadowColor = customFrame.ShadowColor.ToAndroid();
                var shadowOpacity = (float)customFrame.ShadowOpacity;
                var shadowRadius = (float)customFrame.ShadowRadius;

                Elevation = shadowRadius;
                TranslationZ = shadowRadius;
                SetBackgroundColor(shadowColor);
            }
        }
    }

}
