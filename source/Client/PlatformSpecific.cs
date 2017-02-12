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
using System.Threading;
using System.Threading.Tasks;
using TeamSpeak.Sdk;
using TeamSpeak.Sdk.Client;

internal static class PlatformSpecific
{
    private static class NativeWindowsMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)]string procedureName);
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetModuleFileName(IntPtr hModule, [Out]byte[] lpFilename, [In][MarshalAs(UnmanagedType.U4)] int nSize);
    }

    private static class NativeUnixMehods
    {
        [DllImport("__Internal")]
        public static extern IntPtr dlopen(string filename, int flags);
        [DllImport("__Internal")]
        public static extern string dlerror();
        [DllImport("__Internal")]
        public static extern IntPtr dlsym(IntPtr id, string symbol);
        [DllImport("__Internal")]
        public static extern int dlclose(IntPtr id);
        [DllImport("__Internal")]
        public static extern int dladdr(IntPtr addr, ref Dl_info info);
        [DllImport("__Internal")]
        public static extern int uname(IntPtr buf);

        public struct Dl_info
        {
            /// <summary>
            /// Pathname of shared object that contains address
            /// </summary>
            public string dli_fname;

            /// <summary>
            /// Address at which shared object is loaded
            /// </summary>
            public IntPtr dli_fbase;

            /// <summary>
            /// Name of nearest symbol with address lower than addr
            /// </summary>
            public IntPtr dli_sname;

            /// <summary>
            /// Exact address of symbol named in dli_sname
            /// </summary>
            public IntPtr dli_saddr;
        }
    }

    public static void LoadDynamicLibrary(SupportedPlatform platform, string[] possibleNames, out IntPtr handle, out string location)
    {
        foreach (string possibleName in possibleNames)
        {
            if (LoadDynamicLibrary(platform, possibleName, out handle, out location))
                return;
        }
        string message = string.Join(", ", possibleNames);
        switch (platform)
        {
            case SupportedPlatform.Windows:
                throw new DllNotFoundException(message, GetLastWindowsError());
            case SupportedPlatform.Linux:
            case SupportedPlatform.MacOSX:
                throw new DllNotFoundException(message, GetLastErrorUnix());
            default: throw new NotSupportedException();
        }
    }
    private static bool LoadDynamicLibrary(SupportedPlatform platform, string name, out IntPtr handle, out string location)
    {
        switch (platform)
        {
            case SupportedPlatform.Windows:
                handle = NativeWindowsMethods.LoadLibrary(name);
                if (handle == IntPtr.Zero)
                    goto default;
                location = GetLocationWindows(handle) ?? name;
                return true;
            case SupportedPlatform.Linux:
            case SupportedPlatform.MacOSX:
                handle = NativeUnixMehods.dlopen(name, 2 /* RTLD_NOW */);
                if (handle == IntPtr.Zero)
                    goto default;
                location = GetLocationUnix(handle) ?? name;
                return true;
            default:
                handle = IntPtr.Zero;
                location = null;
                return false;
        }
    }

    private static string GetLocationWindows(IntPtr handle)
    {
        byte[] bytes = new byte[260];
        int length = NativeWindowsMethods.GetModuleFileName(handle, bytes, bytes.Length);
        if (length <= 0 || length == bytes.Length)
        {
            Debug.Assert(false, "failed to get path of library");
            return null;
        }
        return Encoding.Default.GetString(bytes, 0, length);
    }

    private static string GetLocationUnix(IntPtr handle)
    {
        IntPtr symbol = NativeUnixMehods.dlsym(handle, "ts3client_freeMemory");
        NativeUnixMehods.Dl_info dl_info = new NativeUnixMehods.Dl_info();
        if (symbol == IntPtr.Zero || NativeUnixMehods.dladdr(symbol, ref dl_info) == 0)
        {
            Debug.Assert(false, "failed to get path of library");
            return null;
        }
        return dl_info.dli_fname;
    }

    public static void GetLibraryMethod<T>(SupportedPlatform platform, IntPtr handle, string name, out T t)
    {
        IntPtr result;
        switch (platform)
        {
            case SupportedPlatform.Windows:
                result = NativeWindowsMethods.GetProcAddress(handle, name);
                if (result == IntPtr.Zero)
                    throw new EntryPointNotFoundException(name, GetLastWindowsError());
                break;
            case SupportedPlatform.Linux:
            case SupportedPlatform.MacOSX:
                result = NativeUnixMehods.dlsym(handle, name);
                if (result == IntPtr.Zero)
                    throw new EntryPointNotFoundException(name, GetLastErrorUnix());
                break;
            default: throw new NotSupportedException();
        }
        t = (T)(object)Marshal.GetDelegateForFunctionPointer(result, typeof(T));
    }

    public static void UnloadDynamicLibrary(SupportedPlatform platform, IntPtr handle)
    {
        switch (platform)
        {
            case SupportedPlatform.Windows:
                if (NativeWindowsMethods.FreeLibrary(handle) == false)
                    throw GetLastWindowsError();
                break;
            case SupportedPlatform.Linux:
            case SupportedPlatform.MacOSX:
                if (NativeUnixMehods.dlclose(handle) != 0)
                    throw GetLastErrorUnix() ?? new InvalidOperationException();
                break;
            default: throw new NotSupportedException();
        }
    }

    //From Managed.Windows.Forms/XplatUI
    static bool IsRunningOnMac()
    {
        IntPtr buf = IntPtr.Zero;
        try
        {
            buf = Marshal.AllocHGlobal(8192);
            // This is a hacktastic way of getting sysname from uname ()
            if (NativeUnixMehods.uname(buf) == 0)
            {
                string os = Marshal.PtrToStringAnsi(buf);
                if (os == "Darwin")
                    return true;
            }
        }
        catch
        {
        }
        finally
        {
            if (buf != IntPtr.Zero)
                Marshal.FreeHGlobal(buf);
        }
        return false;
    }

    private static Exception GetLastWindowsError()
    {
        return new Win32Exception(Marshal.GetLastWin32Error());
    }

    private static Exception GetLastErrorUnix()
    {
        string message = NativeUnixMehods.dlerror();
        return message != null ? new InvalidOperationException(message) : null;
    }

    /// <summary>
    /// Returns the name of the native sdk binary that fits the current environment
    /// </summary>
    /// <param name="names">possible names of the native sdk binary</param>
    /// <param name="platform">detected platform</param>
    /// <returns>true if a matching binary exists</returns>
    public static bool TryGetNativeBinaryName(out string[] names, out SupportedPlatform platform)
    {
        // check if OS is 64-, 32-, or something else bit
        bool is64Bit;
        switch (Native.SizeOfPointer)
        {
            case 8: is64Bit = true; break;
            case 4: is64Bit = false; break;
            default: names = null; platform = 0; return false;
        }
        // check if operating system is supported
        OperatingSystem operatingSystem = Environment.OSVersion;
        switch (operatingSystem.Platform)
        {
            case PlatformID.MacOSX: platform = SupportedPlatform.MacOSX; break;
            case PlatformID.Unix: platform = IsRunningOnMac() ? SupportedPlatform.MacOSX : SupportedPlatform.Linux; break;
            case PlatformID.Win32NT:
                if (operatingSystem.Version >= new Version(5, 1)) // if at least windows xp or newer
                {
                    platform = SupportedPlatform.Windows;
                    break;
                }
                else goto default;
            default: platform = 0; names = null; return false;
        }
        // get name of the binary
        switch (platform)
        {
            case SupportedPlatform.MacOSX:
                names = new string[] { "mac/libts3client.dylib", "libts3client_mac.dylib", "libts3client.dylib" };
                break;
            case SupportedPlatform.Linux:
                names = is64Bit 
                    ? new string[] { "linux/amd64/libts3client.so", "libts3client_linux_amd64.so", "libts3client.so" }
                    : new string[] { "linux/x86/libts3client.so", "libts3client_linux_x86.so", "libts3client.so" };
                break;
            case SupportedPlatform.Windows:
                names = is64Bit
                    ? new string[] { "windows/win64/ts3client.dll", "ts3client_win64.dll", "ts3client.dll" }
                    : new string[] { "windows/win32/ts3client.dll", "ts3client_win32.dll", "ts3client.dll" };
                break;
            default: throw new NotImplementedException();
        }
        return true;
    }
}

