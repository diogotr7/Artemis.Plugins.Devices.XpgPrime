using RGB.NET.Core;
using System;
using System.Collections.Generic;

namespace RGB.NET.Devices.XpgPrime.Keyboard
{
    public class XpgKeyboardDevice : AbstractRGBDevice<XpgDeviceInfo>
    {
        public XpgKeyboardDevice(XpgDeviceInfo deviceInfo, IUpdateQueue updateQueue)
            : base(deviceInfo, updateQueue)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            int x = 0;

            foreach (var led in KeyboardLedMapping.Default)
            {
                AddLed(led.ledId, new Point(x, 0), new Size(19, 19));
                x += 20;
            }
        }
    }
}
