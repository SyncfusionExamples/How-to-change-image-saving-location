using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.IO;
using Android.Graphics;

namespace ImageEditor_GettingStarted.Droid
{
    [Activity(Label = "ImageEditor_GettingStarted", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Instance = this;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public event EventHandler<ActivityResultEventArgs> ActivityResult;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                if (ActivityResult != null)
                    ActivityResult(this, new ActivityResultEventArgs { Intent = data });
            }
        }
    }
    public class ActivityResultEventArgs : EventArgs
    {
        public Intent Intent
        {
            get;
            set;
        }
    }
}