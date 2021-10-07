using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Devices.XpgPrime;
using System.IO;

namespace Artemis.Plugins.Devices.XpgPrime
{
    public class PluginDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        public PluginDeviceProvider(IRgbService rgbService) : base(XpgPrimeDeviceProvider.Instance)
        {
            _rgbService = rgbService;
        }

        public override void Enable()
        {
            XpgPrimeDeviceProvider.PossibleX64NativePaths.Add(Path.Combine(Plugin.Directory.FullName, "x64", "libxpgp_aurora.dll"));

            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            RgbDeviceProvider.Dispose();
        }
    }
}