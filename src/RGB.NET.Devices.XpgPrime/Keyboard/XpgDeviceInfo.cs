using RGB.NET.Core;

namespace RGB.NET.Devices.XpgPrime.Keyboard
{
    public class XpgDeviceInfo : IKeyboardDeviceInfo
    {
        public KeyboardLayoutType Layout => KeyboardLayoutType.Unknown;

        public RGBDeviceType DeviceType => RGBDeviceType.Keyboard;

        public string Manufacturer => "XPG";

        public string Model => "Mage / Summoner";

        public string DeviceName { get; }

        public object? LayoutMetadata { get; set; }

        public XpgDeviceInfo()
        {
            DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
        }
    }
}