using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Devices.XpgPrime;
using System.IO;
using RGB.NET.Core;

namespace Artemis.Plugins.Devices.XpgPrime
{
    public class PluginDeviceProvider : DeviceProvider
    {
        private readonly IDeviceService _deviceService;

        public PluginDeviceProvider(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public override IRGBDeviceProvider RgbDeviceProvider => XpgPrimeDeviceProvider.Instance;

        public override void Enable()
        {
            XpgPrimeDeviceProvider.PossibleX64NativePaths.Add(Path.Combine(Plugin.Directory.FullName, "x64", "libxpgp_aurora.dll"));

            _deviceService.AddDeviceProvider(this);
        }

        public override void Disable()
        {
            _deviceService.RemoveDeviceProvider(this);
            RgbDeviceProvider.Dispose();
        }
    }
}