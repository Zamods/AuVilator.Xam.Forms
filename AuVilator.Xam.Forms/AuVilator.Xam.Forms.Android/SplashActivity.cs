using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AuVilator.Xam.Forms.Droid
{
    [Activity(Label = "AuVilator", Icon = "@mipmap/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            Finish();
            Log.Debug(TAG, "SplashActivity.OnCreate");
            // Create your application here
        }
    }
}