using bluetoothid.iOS;
using CoreBluetooth;

[assembly: Xamarin.Forms.Dependency(typeof(NativeDeviceConverter))]
namespace bluetoothid.iOS
{
    public class NativeDeviceConverter : INativeDevice
    {
        public string GetNativeAddress(object device)
        {
            var notFound = "Not found";
            var dev = device as CBPeripheral;
            if (dev != null)
            {
                if (!string.IsNullOrEmpty(dev.Identifier.ToString()))
                    notFound = dev.Identifier.ToString();
            }

            return notFound;
        }
    }
}
