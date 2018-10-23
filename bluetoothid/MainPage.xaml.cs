using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using System;
using System.Diagnostics;

namespace bluetoothid
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<BluetoothModel> bluetoothModels = new ObservableCollection<BluetoothModel>();

        async Task ScanForDevices()
        {
            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanTimeout = 30000; // 3mins
            adapter.DeviceDiscovered += (s, a) =>
            {
                var newbtd = new BluetoothModel
                {
                    name = !string.IsNullOrEmpty(a.Device.Name) ? a.Device.Name : "not found",
                    id = a.Device.Id.ToString(),
                    address = DependencyService.Get<INativeDevice>().GetNativeAddress(a.Device.NativeDevice),
                };
                bluetoothModels.Add(newbtd);
            };

            adapter.ScanTimeoutElapsed += async (sender, e) =>
            {
                await adapter.StopScanningForDevicesAsync().ContinueWith(async (_) =>
                {
                    if (_.IsCompleted)
                    {
                        await DisplayAlert("Bluetooth Scan", $"Scan complete : {bluetoothModels.Count} devices found", "OK");
                    }
                });
            };

            await adapter.StartScanningForDevicesAsync();
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = bluetoothModels;

            Task.Run(async () => await ScanForDevices());

            bluetoothModels.CollectionChanged += BluetoothModels_CollectionChanged;
        }

        void Handle_Refreshing(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lstDevices.ItemsSource = null;
                lstDevices.ItemsSource = bluetoothModels;
                lstDevices.IsRefreshing = false;
            });
        }

        void BluetoothModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lstDevices.ItemsSource = null;
                lstDevices.ItemsSource = bluetoothModels;
                lstDevices.IsRefreshing = false;
            });
        }

        void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (BluetoothModel)e.SelectedItem;
            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;
            Device.BeginInvokeOnMainThread(async () =>
            {
                var connect = await adapter.ConnectToKnownDeviceAsync(new Guid(item.id));
                if (connect != null)
                {
                    Debug.WriteLine($"Connection attempted, state = {connect?.State}");
                    var services = await connect.GetServicesAsync();
                    if (services != null)
                    {
                        foreach (var s in services)
                        {
                            Debug.WriteLine($"service name {s.Name}");
                            var servicechars = await s.GetCharacteristicsAsync();
                            if (s != null)
                            {
                                foreach (var c in servicechars)
                                    Debug.WriteLine($"service characteristics : {c.Name}, can read {c.CanRead}, can write {c.CanWrite}, can update {c.CanUpdate}");
                            }
                        }
                    }
                }
            });
        }
    }
}
