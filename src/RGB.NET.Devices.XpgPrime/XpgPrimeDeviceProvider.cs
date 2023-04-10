using RGB.NET.Core;
using RGB.NET.Devices.XpgPrime.Keyboard;
using RGB.NET.Devices.XpgPrime.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Devices.XpgPrime
{
    public class XpgPrimeDeviceProvider : AbstractRGBDeviceProvider
    {
        private static XpgPrimeDeviceProvider? _instance;
        public static XpgPrimeDeviceProvider Instance => _instance ?? new XpgPrimeDeviceProvider();

        public XpgPrimeDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(XpgPrimeDeviceProvider)}");
            _instance = this;
        }

        public static List<string> PossibleX64NativePaths { get; } = new List<string> { "x64/libxpgp_aurora.dll" };

        #region Overrides of AbstractRGBDeviceProvider

        protected override void InitializeSDK()
        {
            _XpgPrimeSdk.Reload();
            var initResult = _XpgPrimeSdk.Initialize();
            if (initResult != 0) Throw(new RGBDeviceException("Failed to initialize XpgPrimeSDK."), true);
        }

        /// <inheritdoc />
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            yield return new XpgKeyboardDevice(new XpgDeviceInfo(), new XpgPrimeUpdateQueue(GetUpdateTrigger()));
        }

        #endregion
    }
}