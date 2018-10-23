using bluetoothid.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(NativeDeviceConverter))]
namespace bluetoothid.Droid
{
    public class NativeDeviceConverter : INativeDevice
    {
        public string GetNativeAddress(object device)
        {
            var notFound = "Not found";
            var dev = device as Android.Bluetooth.BluetoothDevice;
            if (dev != null)
            {
                if (!string.IsNullOrEmpty(dev.Address))
                    notFound = dev.Address;
            }

            return notFound;
        }
    }
}
