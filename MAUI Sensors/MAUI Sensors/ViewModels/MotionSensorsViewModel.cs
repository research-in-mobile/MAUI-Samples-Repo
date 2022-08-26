using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MAUI_Sensors.ViewModels
{
    public partial class MotionSensorsViewModel : ObservableObject
    {
        private SensorSpeed _sensorSpeed = SensorSpeed.UI; //Change this for faster update

        [ObservableProperty]
        private bool _hasPermissions;
        [ObservableProperty]
        private string _accelerometerReading;
        [ObservableProperty]
        private string _gyroReading;
        [ObservableProperty]
        private string _magnetometerReading;
        [ObservableProperty]
        private string _compassReading;
        [ObservableProperty]
        private string _orientationReading;
        [ObservableProperty]
        private int _shakeDetectedCount;


        public MotionSensorsViewModel()
        {
            ToggleAccelerometerReading();
            ToggleGyroscopeReading();
            ToggleMagnetometerReading();
            ToggleCompassReading();
            ToggleOrientationReading();
            ToggleShakeDetection();
        }

        private async Task<bool> HasSensorPremission()
        {
            var hasPermissions = await Permissions.CheckStatusAsync<Permissions.Sensors>() == PermissionStatus.Granted;

            if (!hasPermissions)
            {
                var permissions = await Permissions.RequestAsync<Permissions.Sensors>();
                hasPermissions = permissions == PermissionStatus.Granted;
            }

            return hasPermissions;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            //Acceleration m/s
            AccelerometerReading = $"{e.Reading}";
        }
        private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            var data = e.Reading;
            // Process Angular Velocity X, Y, and Z reported in rad/s
            GyroReading = $"{e.Reading}";
        }
        private void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            var data = e.Reading;
            // Process MagneticField X, Y, and Z
            MagnetometerReading = $"{e.Reading}";

        }
        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var data = e.Reading;
            //360 degrees
            CompassReading = $"{e.Reading.HeadingMagneticNorth}";
        }
        private void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            var data = e.Reading;
            // Process Orientation quaternion (X, Y, Z, and W)
            OrientationReading = $"{e.Reading}";
        }
        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            ShakeDetectedCount++;
        }


        public async void ToggleShakeDetection()
        {
            try
            {
                if (Accelerometer.IsSupported)
                {
                    if (!await HasSensorPremission()) return;


                    if (!Accelerometer.IsMonitoring)
                    {
                        Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
                    }
                    else
                    {
                        Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert($"Accelerometer Not Supported", "Service not supported.", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert($"Error", ex.ToString(), "Ok");
            }
        }
        public async void ToggleOrientationReading()
        {
            try
            {
                if (OrientationSensor.IsSupported)
                {

                    if (!await HasSensorPremission()) return;

                    if (!OrientationSensor.IsMonitoring)
                    {
                        OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
                        OrientationSensor.Start(_sensorSpeed);
                    }
                    else
                    {
                        OrientationSensor.Stop();
                        OrientationSensor.ReadingChanged -= OrientationSensor_ReadingChanged;
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert($"OrientationSensor  Not Supported", "Service not supported.", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert($"Error", ex.ToString(), "Ok");
            }
        }
        public async void ToggleCompassReading()
        {
            try
            {
                if (Compass.IsSupported)
                {

                    if (!await HasSensorPremission()) return;

                    if (!Compass.IsMonitoring)
                    {
                        Compass.ReadingChanged += Compass_ReadingChanged;
                        Compass.Default.Start(_sensorSpeed, applyLowPassFilter: true);
                        //Compass.Start(_sensorSpeed);
                    }
                    else
                    {
                        Compass.Stop();
                        Compass.ReadingChanged -= Compass_ReadingChanged;
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert($"Compass  Not Supported", "Service not supported.", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert($"Error", ex.ToString(), "Ok");
            }
        }
        public async void ToggleAccelerometerReading()
        {
            try
            {
                if (Accelerometer.IsSupported)
                {
                    if (!await HasSensorPremission()) return;


                    if (!Accelerometer.IsMonitoring)
                    {
                        Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                        Accelerometer.Start(_sensorSpeed);
                    }
                    else
                    {
                        Accelerometer.Stop();
                        Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert($"Accelerometer Not Supported", "Service not supported.", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert($"Error", ex.ToString(), "Ok");
            }
        }
        public async void ToggleGyroscopeReading()
        {
            try
            {
                if (Gyroscope.IsSupported)
                {
                    if (!await HasSensorPremission()) return;


                    if (!Gyroscope.IsMonitoring)
                    {

                        Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
                        Gyroscope.Start(_sensorSpeed);
                    }
                    else
                    {
                        Gyroscope.Stop();
                        Gyroscope.ReadingChanged -= Gyroscope_ReadingChanged;
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert($"Gyro Not Supported", "Service not supported.", "Ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert($"Error", ex.ToString(), "Ok");
            }
        }
        public async void ToggleMagnetometerReading()
        {
            try
            {

                if (Magnetometer.IsSupported)
                {
                    if (!await HasSensorPremission()) return;


                    if (!Magnetometer.IsMonitoring)
                    {

                        Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
                        Magnetometer.Start(_sensorSpeed);
                    }
                    else
                    {
                        Magnetometer.Stop();
                        Magnetometer.ReadingChanged -= Magnetometer_ReadingChanged;
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert($"Magnetometer Not Supported", "Service not supported.", "Ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert($"Error", ex.ToString(), "Ok");
            }
        }

    }
}
