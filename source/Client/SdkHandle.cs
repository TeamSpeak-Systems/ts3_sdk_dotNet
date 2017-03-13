using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TeamSpeak.Sdk.Client
{
    internal class SdkHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private static AutoResetEvent DllUnloaded = new AutoResetEvent(true);

        public static SdkHandle Load(SupportedPlatform platform, string[] possibleNames)
        {
            DllUnloaded.WaitOne();
            IntPtr handle;
            string location;
            PlatformSpecific.LoadDynamicLibrary(platform, possibleNames, out handle, out location);
            return new SdkHandle(handle, platform, location);
        }

        public string Location { get; }
        public SupportedPlatform Platform { get; }

        private SdkHandle(IntPtr handle, SupportedPlatform platform, string location)
            : base(true)
        {
            SetHandle(handle);
            Platform = platform;
            Location = location;
        }

        public void GetLibraryMethod<T>(string name, out T t)
        {
            PlatformSpecific.GetLibraryMethod(Platform, handle, name, out t);
        }

        protected override bool ReleaseHandle()
        {
            bool result = true;
            try
            {
                NativeMethods.DestroyClientLibDelegate destroyClientLib;
                GetLibraryMethod("ts3client_destroyClientLib", out destroyClientLib);
                destroyClientLib();
            }
            catch
            {
                result = false;
            }
            try
            {
                PlatformSpecific.UnloadDynamicLibrary(Platform, handle);
            }
            catch
            {
                result = false;
            }
            try
            {
                DllUnloaded.Set();
            }
            catch (ObjectDisposedException)  { /* nop */ }
            return result;
        }
    }
}
