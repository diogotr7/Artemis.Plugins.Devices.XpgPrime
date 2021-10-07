using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RGB.NET.Devices.XpgPrime.Native
{
    public static class _XpgPrimeSdk
    {
        #region Libary Management

        private static IntPtr _dllHandle = IntPtr.Zero;

        /// <summary>
        /// Reloads the SDK.
        /// </summary>
        internal static void Reload()
        {
            UnloadXpgPrimeGSDK();
            LoadXpgPrimeGSDK();
        }

        private static void LoadXpgPrimeGSDK()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            List<string> possiblePathList = Environment.Is64BitProcess ? XpgPrimeDeviceProvider.PossibleX64NativePaths : throw new Exception();
            string? dllPath = possiblePathList.FirstOrDefault(File.Exists);
            if (dllPath == null) throw new RGBDeviceException($"Can't find the Xpg Prime SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            _dllHandle = LoadLibrary(dllPath);
            if (_dllHandle == IntPtr.Zero) throw new RGBDeviceException($"Xpg Prime SDK LoadLibrary failed with error code {Marshal.GetLastWin32Error()}");

            _xpgPrimeInitializePointer = (XpgPrimeInitializePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "Initialize"), typeof(XpgPrimeInitializePointer));
            _xpgPrimeResetPointer = (XpgPrimeResetPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "Reset"), typeof(XpgPrimeResetPointer));
            _xpgPrimeShutdownPointer = (XpgPrimeShutdownPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "Shutdown"), typeof(XpgPrimeShutdownPointer));
            _xpgPrimeUpdateDevicePointer = (XpgPrimeUpdateDevicePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "UpdateDevice"), typeof(XpgPrimeUpdateDevicePointer));
        }

        internal static void UnloadXpgPrimeGSDK()
        {
            if (_dllHandle == IntPtr.Zero) return;

            Shutdown();

            // ReSharper disable once EmptyEmbeddedStatement - DarthAffe 20.02.2016: We might need to reduce the internal reference counter more than once to set the library free
            while (FreeLibrary(_dllHandle)) ;
            _dllHandle = IntPtr.Zero;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr dllHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr dllHandle, string name);

        #endregion

        #region SDK-METHODS

        #region Pointers

        private static XpgPrimeInitializePointer? _xpgPrimeInitializePointer;
        private static XpgPrimeResetPointer? _xpgPrimeResetPointer;
        private static XpgPrimeShutdownPointer? _xpgPrimeShutdownPointer;
        private static XpgPrimeUpdateDevicePointer? _xpgPrimeUpdateDevicePointer;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int XpgPrimeInitializePointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void XpgPrimeResetPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void XpgPrimeShutdownPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int XpgPrimeUpdateDevicePointer(byte[] array, int size);

        #endregion

        // ReSharper disable EventExceptionNotDocumented

        internal static int Initialize() => (_xpgPrimeInitializePointer ?? throw new RGBDeviceException("The Xpg Prime SDK is not initialized.")).Invoke();

        internal static void Reset() => (_xpgPrimeResetPointer ?? throw new RGBDeviceException("The Xpg Prime SDK is not initialized.")).Invoke();

        internal static void Shutdown() => (_xpgPrimeShutdownPointer ?? throw new RGBDeviceException("The Xpg SDK Prime is not initialized.")).Invoke();

        internal static int UpdateDevice(byte[] array, int size) => (_xpgPrimeUpdateDevicePointer ?? throw new RGBDeviceException("The Xpg Prime SDK is not initialized.")).Invoke(array, size);

        #endregion
    }
}
