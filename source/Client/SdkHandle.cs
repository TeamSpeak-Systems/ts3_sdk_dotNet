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
        public static SdkHandle Load(SupportedPlatform platform, string[] possibleNames)
        {
            IntPtr handle;
            string location;
            PlatformSpecific.LoadDynamicLibrary(platform, possibleNames, out handle, out location);
            return new SdkHandle(handle, platform, location);
        }

        public string Location { get; }
        public SupportedPlatform Platform { get; }
        private volatile int State;

        private SdkHandle(IntPtr handle, SupportedPlatform platform, string location)

            : base(true)
        {
            SetHandle(handle);
            Platform = platform;
            Location = location;
            State = 0;
        }

        public void GetLibraryMethod<T>(string name, out T t)
        {
            PlatformSpecific.GetLibraryMethod(Platform, handle, name, out t);
        }

        public void WaitForUnloadingFinished()
        {
            while (State != 1) Thread.Yield();
        }

        protected override bool ReleaseHandle()
        {
            try
            {
                NativeMethods.DestroyClientLibDelegate destroyClientLib;
                GetLibraryMethod("ts3client_destroyClientLib", out destroyClientLib);
                destroyClientLib();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                PlatformSpecific.UnloadDynamicLibrary(Platform, handle);
                State = 1;
            }
        }
    }
}
