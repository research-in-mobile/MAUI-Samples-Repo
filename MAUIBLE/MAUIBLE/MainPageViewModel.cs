using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;

using System.Collections.ObjectModel;


namespace MAUIBLE
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<IDevice> _devices;
        [ObservableProperty]
        private ObservableCollection<IDevice> _connectedOrPairedDevices;

        [ObservableProperty]
        private bool _hasPermissions;
        [ObservableProperty]
        private bool _isBusy;

        private string _lastConnectedDevice = null;
        public string LastConnectedDevice
        {
            get
            {
                if (_lastConnectedDevice == null)
                {
                    if (Preferences.ContainsKey(nameof(_lastConnectedDevice)))
                    {
                        var resultString = Preferences.Get(nameof(_lastConnectedDevice), "");
                        if (!string.IsNullOrWhiteSpace(resultString))
                        {
                            _lastConnectedDevice = JsonConvert.DeserializeObject<string>(resultString);
                        }
                    }
                }

                return _lastConnectedDevice;
            }
            set
            {
                if (value == null) return;

                _lastConnectedDevice = value;

                if (Preferences.ContainsKey(nameof(_lastConnectedDevice)))
                {
                    Preferences.Remove(nameof(_lastConnectedDevice));
                }

                var dateString = JsonConvert.SerializeObject(value);
                Preferences.Set(nameof(_lastConnectedDevice), dateString);
            }
        }

        private readonly IBluetoothLE _ble;
        private readonly IAdapter _adapter;
        public MainPageViewModel()
        {
            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            _devices = new ObservableCollection<IDevice>();
            _connectedOrPairedDevices = new ObservableCollection<IDevice>();

            _ble.StateChanged += async (s, e) =>
            {
                await App.Current.MainPage.DisplayAlert("Error", $"The bluetooth state changed to {e.NewState}", "Ok");
            };

            _adapter.DeviceDiscovered += (s, a) =>
            {
                if (!string.IsNullOrEmpty(a.Device.Name) && !_devices.Contains(a.Device))
                {
                    _devices.Add(a.Device);
                }
            };

            TryConnectToLastDevice();
        }

        private async void TryConnectToLastDevice()
        {
            try
            {
                var canConnect = Guid.TryParse(LastConnectedDevice, out var deviceId);
                if (canConnect)
                {
                    var device = await _adapter.ConnectToKnownDeviceAsync(deviceId);
                    var services = await device.GetServicesAsync();

                    await App.Current.MainPage.DisplayAlert($"Connected to {device.Name}", JsonConvert.SerializeObject(services), "Ok");
                }
            }
            catch (DeviceConnectionException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");

            }
        }

        [RelayCommand]
        private async void ScanBLE()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                _devices.Clear();
                _connectedOrPairedDevices.Clear();

                HasPermissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted;

                if (!HasPermissions)
                {
                    var permissions = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    HasPermissions = permissions == PermissionStatus.Granted;
                }

                if (HasPermissions)
                {

                    var systemDevices = _adapter.GetSystemConnectedOrPairedDevices();

                    foreach (var device in systemDevices)
                    {
                        _connectedOrPairedDevices.Add(device);
                    }

                    if (!_ble.Adapter.IsScanning)
                    {
                        await _adapter.StartScanningForDevicesAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");

            }
            IsBusy = false;
        }
        [RelayCommand]
        private async void Connect(IDevice device)
        {

            try
            {
                await _adapter.ConnectToDeviceAsync(device);
                var services = await device.GetServicesAsync();

                if (services != null)
                {
                    LastConnectedDevice = device.Id.ToString();

                    //var characteristics = await services[2].GetCharacteristicsAsync();

                    await App.Current.MainPage.DisplayAlert($"Connected to {device.Name}", JsonConvert.SerializeObject(services), "Ok");
                }
                else
                    await App.Current.MainPage.DisplayAlert($"Connected to {device.Name}", "No available services found.", "Ok");
            }
            catch (DeviceConnectionException e)
            {
                await App.Current.MainPage.DisplayAlert("Sorry", $"Colud not connect to {device.Name}.", "Ok");

            }
        }
    }
}
