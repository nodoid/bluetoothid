using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using Plugin.Permissions;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Xamarin.Forms;

namespace bluetoothid.Droid
{
    [Activity(Label = "bluetoothid", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        static readonly int REQUEST_BLUETOOTH = 0, REQUEST_BLUETOOTHADMIN = 1;

        Android.Views.View layout;

        static string[] PERMISSIONS_BLUETOOTH = { Manifest.Permission.Bluetooth };
        static string[] PERMISSIONS_BLUETOOTHADMIN = { Manifest.Permission.BluetoothAdmin };

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
                                                        [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                RequestBluetoothPermission();
            }

            LoadApplication(new App());
        }

        void RequestBluetoothPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Bluetooth))
            {
                Console.WriteLine("Show bluetooth");
                Snackbar.Make(GetSnackbarAnchorView(), "Let me use bluetooth",
                              Snackbar.LengthIndefinite).SetAction("OK", v => ActivityCompat.RequestPermissions(this, PERMISSIONS_BLUETOOTH, REQUEST_BLUETOOTH)).Show();

            }
            else
            {
                ActivityCompat.RequestPermissions(this, PERMISSIONS_BLUETOOTH, REQUEST_BLUETOOTH);
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.BluetoothAdmin))
            {
                Console.WriteLine("Show bluetoothadmin");
                Snackbar.Make(GetSnackbarAnchorView(), "Let me use bluetooth admin",
                              Snackbar.LengthIndefinite).SetAction("OK", v => ActivityCompat.RequestPermissions(this, PERMISSIONS_BLUETOOTHADMIN, REQUEST_BLUETOOTHADMIN)).Show();

            }
            else
            {
                ActivityCompat.RequestPermissions(this, PERMISSIONS_BLUETOOTHADMIN, REQUEST_BLUETOOTHADMIN);
            }
        }

        Android.Views.View GetSnackbarAnchorView()
        {
            var a = (Activity)Forms.Context;
            var v3 = a.FindViewById(Android.Resource.Id.Content);
            return v3;
        }
    }
}