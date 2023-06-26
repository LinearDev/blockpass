using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Blockpasstest.iOS;
using Blockpasstest.Assets;
using CoreGraphics;
using LocalAuthentication;
using static Blockpasstest.iOS.BorderedEntryRenderer;
using System.Threading.Tasks;
using ColorPicker.iOS;

[assembly: ExportRenderer(typeof(BorderedEntry), typeof(BorderedEntryRenderer))]
[assembly: ExportRenderer(typeof(BFrame), typeof(BFrameRenderer))]
namespace Blockpasstest.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ColorPickerEffects.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            Rg.Plugins.Popup.Popup.Init();
            this.AuthenticateWithBiometricsAsync();

            return base.FinishedLaunching(app, options);
        }

        public async Task<bool> AuthenticateWithBiometricsAsync()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var context = new LAContext();
                NSError error = null;

                if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error))
                {
                    var biometricType = context.BiometryType;

                    if (biometricType == LABiometryType.TouchId)
                    {
                        var result = await context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, "Authentication required");

                        return result.Item1 == true;
                    }
                    else
                    {
                        // Touch ID not available or supported
                        return false;
                    }
                }
                else
                {
                    // Biometric authentication not available
                    return false;
                }
            }
            else
            {
                // Biometric authentication not supported on non-iOS platforms
                return false;
            }
        }
    }

    public class BorderedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (BorderedEntry)Element;

                Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                Control.LeftViewMode = UITextFieldViewMode.Always;

                Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                Control.ReturnKeyType = UIReturnKeyType.Done;

                // Radius for the curves  
                Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);

                // Thickness of the Border Color  
                Control.Layer.BorderColor = view.BorderColor.ToCGColor(); 
                Control.Layer.BorderWidth = view.BorderWidth;
                Control.ClipsToBounds = true;
                Control.AutocapitalizationType = UITextAutocapitalizationType.None;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsFocused" && Control != null && Element != null)
            {
                if (Element.IsFocused)
                {
                    Control.BecomeFirstResponder();
                }
                else
                {
                    Control.ResignFirstResponder();
                }
            }
        }
    }

    public class BFrameRenderer : FrameRenderer
    {
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
                var shadowColor = customFrame.ShadowColor.ToUIColor();
                var shadowOpacity = (float)customFrame.ShadowOpacity;
                var shadowRadius = (float)customFrame.ShadowRadius;

                Layer.ShadowColor = shadowColor.CGColor;
                Layer.ShadowOpacity = shadowOpacity;
                Layer.ShadowRadius = shadowRadius;
                Layer.ShadowOffset = new CGSize(0, 0);
                Layer.MasksToBounds = false;
                Layer.BackgroundColor = customFrame.BackgroundColor.ToCGColor();
                Layer.CornerRadius = (float)customFrame.CornerRadius;
            }
        }
    }
}

