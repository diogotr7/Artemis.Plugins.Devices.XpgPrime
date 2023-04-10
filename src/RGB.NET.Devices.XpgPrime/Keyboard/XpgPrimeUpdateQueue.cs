using RGB.NET.Core;
using RGB.NET.Devices.XpgPrime.Native;
using System;

namespace RGB.NET.Devices.XpgPrime.Keyboard
{
    public class XpgPrimeUpdateQueue : UpdateQueue
    {
        private const int COLOR_COUNT = 108; // highest value we use is FN_Key (107)
        private const int ARRAY_SIZE = 4 * COLOR_COUNT;
        private readonly byte[] array;

        public XpgPrimeUpdateQueue(IDeviceUpdateTrigger trigger) : base(trigger)
        {
            array = new byte[ARRAY_SIZE];
        }

        protected override bool Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            foreach (var (key, color) in dataSet)
            {
                var idx = (int)key;

                array[(4 * idx) + 0] = color.GetR();
                array[(4 * idx) + 1] = color.GetG();
                array[(4 * idx) + 2] = color.GetB();
                array[(4 * idx) + 3] = color.GetA();
            }

            _XpgPrimeSdk.UpdateDevice(array, array.Length);
            return true;
        }
    }
}