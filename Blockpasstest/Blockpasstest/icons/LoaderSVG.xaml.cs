using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Blockpasstest.icons
{	
	public partial class LoaderSVG : ContentView
	{	
		public LoaderSVG ()
		{
			InitializeComponent ();
            var rotateAnimation = new Animation(v => Loader.Rotation = v, 0, 360);
            rotateAnimation.Commit(Loader, "RotationAnimation", length: 1000, easing: Easing.Linear, repeat: () => true);
        }
    }
}

