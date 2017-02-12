using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TeamSpeak.Sdk;
using TeamSpeak.Sdk.Client;

internal static class PlatformSpecific
{
    private static class NativeUnixMehods
    {
        [DllImport("__Internal")]
        public static extern IntPtr dlopen(string filename, int flags);
        [DllImport("__Internal")]
        public static extern IntPtr dlerror();
        [DllImport("__Internal")]
        public static extern IntPtr dlsym(IntPtr id, string symbol);
        [DllImport("__Internal")]
        public static extern int dlclose(IntPtr id);
    }

    public static void LoadDynamicLibrary(SupportedPlatform platform, string[] possibleNames, out IntPtr handle, out string location)
    {
        if (platform != SupportedPlatform.Android) throw new NotSupportedException();
        foreach (string possibleName in possibleNames)
        {
            handle = NativeUnixMehods.dlopen(possibleName, 2 /* RTLD_NOW */);
            if (handle != IntPtr.Zero)
            {
                location = possibleName;
                return;
            }
        }
        string message = string.Join(", ", possibleNames);
        throw new DllNotFoundException(message, GetLastError());
    }

    public static void GetLibraryMethod<T>(SupportedPlatform platform, IntPtr handle, string name, out T t)
    {
        if (platform != SupportedPlatform.Android) throw new NotSupportedException();
        IntPtr result = NativeUnixMehods.dlsym(handle, name);
        if (result == IntPtr.Zero)
            throw new EntryPointNotFoundException(name, GetLastError());
        t = (T)(object)Marshal.GetDelegateForFunctionPointer(result, typeof(T));
    }

    public static void UnloadDynamicLibrary(SupportedPlatform platform, IntPtr handle)
    {
        if (platform != SupportedPlatform.Android) throw new NotSupportedException();
        if (NativeUnixMehods.dlclose(handle) != 0)
            throw GetLastError() ?? new InvalidOperationException();
    }

    private static Exception GetLastError()
    {
        IntPtr pointer = NativeUnixMehods.dlerror();
        return pointer != IntPtr.Zero ? new InvalidOperationException(Marshal.PtrToStringAnsi(pointer)) : null;
    }

    /// <summary>
    /// Returns the name of the native sdk binary that fits the current environment
    /// </summary>
    /// <param name="names">possible names of the native sdk binary</param>
    /// <param name="platform">detected platform</param>
    /// <returns>true if a matching binary exists</returns>
    public static bool TryGetNativeBinaryName(out string[] names, out SupportedPlatform platform)
    {
        platform = SupportedPlatform.Android;

        // check if platform is 64-, 32-, or something else bit
        switch (Native.SizeOfPointer)
        {
            case 8: case 4: break;
            default: names = null; platform = 0; return false;
        }

        names = new string[] { "libts3client.so", "libts3client_android.so" };
        return true;
    }
}