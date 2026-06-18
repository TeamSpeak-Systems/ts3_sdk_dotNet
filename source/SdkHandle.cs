using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
#endif

namespace TeamSpeak.Sdk
{
    internal class SdkHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        // Allow up to 2 concurrent library loads (one for the client library, one for the server library).
        // Reloading the same library type while a previous instance is still being destroyed is prevented
        // by Library.Initialize(), which guards against double-init with its own lock.
        private static SemaphoreSlim LoadSlots = new SemaphoreSlim(2, 2);

        public static SdkHandle LoadClientLibrary(SupportedPlatform platform, string[] possibleNames)
        {
            return Load(platform, possibleNames, "ts3client_destroyClientLib");
        }

        public static SdkHandle LoadServerLibrary(SupportedPlatform platform, string[] possibleNames)
        {
            return Load(platform, possibleNames, "ts3server_destroyServerLib");
        }

        private static SdkHandle Load(SupportedPlatform platform, string[] possibleNames, string destroyLibraryApi)
        {
            LoadSlots.Wait();
            IntPtr handle;
            string location;
#if UNITY_ANDROID && !UNITY_EDITOR
            handle = (IntPtr)1;
            location = TeamSpeak.Sdk.Client.Unity.UnityAndroidHelper.getNativeLibraryDir();
#else
            PlatformSpecific.LoadDynamicLibrary(platform, possibleNames, out handle, out location);
#endif
            return new SdkHandle(handle, platform, location, destroyLibraryApi);
        }

        public string Location { get; }
        public SupportedPlatform Platform { get; }
        private readonly string DestroyLibraryApi;

        private SdkHandle(IntPtr handle, SupportedPlatform platform, string location, string destroyLibraryApi)
            : base(true)
        {
            SetHandle(handle);
            Platform = platform;
            Location = location;
            DestroyLibraryApi = destroyLibraryApi;
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
#if UNITY_EDITOR || !UNITY_ANDROID
                DestroyLibraryDelegate destroyLibrary;
                GetLibraryMethod(DestroyLibraryApi, out destroyLibrary);
                destroyLibrary();
#elif UNITY_ANDROID
                AndroidJavaClass teamSpeakWrapper = new AndroidJavaClass("com.teamspeak.unity.TeamSpeakWrapper");
                teamSpeakWrapper.CallStatic("DestroyClientLib");
#endif
            }
            catch
            {
                result = false;
            }
#if UNITY_EDITOR || !UNITY_ANDROID
            try
            {
                PlatformSpecific.UnloadDynamicLibrary(Platform, handle);
            }
            catch
            {
                result = false;
            }
#endif
            try
            {
                LoadSlots.Release();
            }
            catch (ObjectDisposedException) { /* nop */ }
            return result;
        }

        [UnmanagedFunctionPointer(Native.CallingConvention)]
        private delegate Error DestroyLibraryDelegate();
    }
}
