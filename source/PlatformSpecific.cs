using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace TeamSpeak.Sdk
{
    internal static class PlatformSpecific
    {
        private const string PackageName = "com.teamspeak.sdk";
        private const string PackageVendor = "TeamSpeak";
        private const string PackageShortName = "TS_SDK";

        private const string AssetPath = "Assets/" + PackageName + "/Plugins";
        private const string AssetStorePath = "Assets/" + PackageVendor + "/" + PackageShortName + "/Plugins";
        private const string PackagePath = "Packages/" + PackageName + "/Plugins";

        private const string WindowsClientLibName = "teamspeak_sdk_client.dll";
        private const string LinuxClientLibName = "libteamspeak_sdk_client.so";
        private const string AppleClientLibName = "libteamspeak_sdk_client.dylib";

        private const int RTLD_NOW = 2;

        private static class NativeWindowsMethods
        {
            [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "LoadLibraryW", CallingConvention = CallingConvention.Winapi)]
            public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
            /*[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
            public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);*/
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procedureName);
            [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
            public static extern bool FreeLibrary(IntPtr hModule);
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
            public static extern int GetModuleFileName(IntPtr hModule, [Out] byte[] lpFilename, [In][MarshalAs(UnmanagedType.U4)] int nSize);
        }

        private static class NativeUnixMehods
        {
#if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_WSA || UNITY_UWP
            public static IntPtr dlopen(string filename, int flags) => IntPtr.Zero;
            public static IntPtr dlerror() => IntPtr.Zero;
            public static IntPtr dlsym(IntPtr id, string symbol) => IntPtr.Zero;
            public static int dlclose(IntPtr id) => 0;
            public static int dladdr(IntPtr addr, ref Dl_info info) => 0;
            public static int uname(IntPtr buf) => 0;
#else
            [DllImport("__Internal")]
            public static extern IntPtr dlopen(string filename, int flags);
            /* Reminder: You may see dlerror with string return type, don't try.
             * It'll crash on iOS. */
            [DllImport("__Internal")]
            public static extern IntPtr dlerror();
            [DllImport("__Internal")]
            public static extern IntPtr dlsym(IntPtr id, string symbol);
            [DllImport("__Internal")]
            public static extern int dlclose(IntPtr id);
            [DllImport("__Internal")]
            public static extern int dladdr(IntPtr addr, ref Dl_info info);
            [DllImport("__Internal")]
            public static extern int uname(IntPtr buf);
#endif

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
                case SupportedPlatform.Android:
                case SupportedPlatform.Linux:
                case SupportedPlatform.MacOSX:
                case SupportedPlatform.iOS:
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
                case SupportedPlatform.Android:
                case SupportedPlatform.Linux:
                case SupportedPlatform.MacOSX:
                case SupportedPlatform.iOS:
                    handle = NativeUnixMehods.dlopen(name, RTLD_NOW);
                    if (handle == IntPtr.Zero)
                        goto default;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
                    location = name; // dladdr-based path resolution is unavailable under Unity IL2CPP
#else
                    location = GetLocationUnix(handle) ?? name;
#endif
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
                return null;
            return Encoding.Default.GetString(bytes, 0, length);
        }

        private static string GetLocationUnix(IntPtr handle)
        {
            IntPtr symbol = NativeUnixMehods.dlsym(handle, "ts3client_freeMemory");
            if (symbol == IntPtr.Zero)
                symbol = NativeUnixMehods.dlsym(handle, "ts3server_freeMemory");
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
                case SupportedPlatform.Android:
                case SupportedPlatform.Linux:
                case SupportedPlatform.MacOSX:
                case SupportedPlatform.iOS:
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
                case SupportedPlatform.Android:
                case SupportedPlatform.Linux:
                case SupportedPlatform.MacOSX:
                case SupportedPlatform.iOS:
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
                // Read sysname from uname() to detect the operating system.
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

        private static bool? _isAndroid;
        static bool IsRunningOnAndroid()
        {
            if (_isAndroid != null) return (bool)_isAndroid;
#if __ANDROID__ || (UNITY_ANDROID && !UNITY_EDITOR)
            _isAndroid = true;
            return (bool)_isAndroid;
#else
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = "getprop";
                process.StartInfo.Arguments = "ro.build.user";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                try
                {
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    _isAndroid = string.IsNullOrEmpty(output) ? (bool?)false : (bool?)true;
                }
                catch
                {
                    _isAndroid = false;
                }
                return (bool)_isAndroid;
            }
#endif
        }

        private static Exception GetLastWindowsError()
        {
            return new Win32Exception(Marshal.GetLastWin32Error());
        }

        private static Exception GetLastErrorUnix()
        {
            string message = Marshal.PtrToStringAnsi(NativeUnixMehods.dlerror());
            return message != null ? new InvalidOperationException(message) : null;
        }

        private static bool TryGetPlatform(out SupportedPlatform platform, out bool is64Bit)
        {
            // determine if application is running in 64 or 32bit mode
            switch (Native.SizeOfPointer)
            {
                case 8: is64Bit = true; break;
                case 4: is64Bit = false; break;
                default: is64Bit = false; platform = 0; return false;
            }

#if UNITY_IOS && !UNITY_EDITOR
            platform = SupportedPlatform.iOS;
#else
            // get operating system
            OperatingSystem operatingSystem = Environment.OSVersion;
            switch (operatingSystem.Platform)
            {
                case PlatformID.MacOSX: platform = SupportedPlatform.MacOSX; break;
                case PlatformID.Unix:
                    if (IsRunningOnMac()) platform = SupportedPlatform.MacOSX;
                    else if (IsRunningOnAndroid()) platform = SupportedPlatform.Android;
                    else platform = SupportedPlatform.Linux;
                    break;
                case PlatformID.Win32NT:
                    if (operatingSystem.Version >= new Version(5, 1)) // if at least windows xp or newer
                    {
                        platform = SupportedPlatform.Windows;
                        break;
                    }
                    else goto default;
                default: platform = 0; return false;
            }
#endif
            return true;
        }

        /// <summary>
        /// Returns the name of the native sdk binary that fits the current environment
        /// </summary>
        /// <param name="names">possible names of the native sdk binary</param>
        /// <param name="platform">detected platform</param>
        /// <returns>true if a matching binary exists</returns>
        public static bool TryGetNativeBinaryNameClient(out string[] names, out SupportedPlatform platform)
        {
            bool is64Bit;
            if (TryGetPlatform(out platform, out is64Bit) == false)
            {
                names = null;
                return false;
            }

            // get name of the binary
            switch (platform)
            {
                case SupportedPlatform.MacOSX:
                    names = new string[] { "libteamspeak_sdk_client.dylib", "ts3client.framework/ts3client", "ts3client.framework/Versions/A/ts3client", "mac/libts3client.dylib", "libts3client_mac.dylib", "libts3client.dylib" };
                    break;
                case SupportedPlatform.Linux:
                    names = is64Bit
                        ? new string[] { "libteamspeak_sdk_client.so", "linux/amd64/libts3client.so", "libts3client_linux_amd64.so", "libts3client.so" }
                        : new string[] { "libteamspeak_sdk_client.so", "linux/x86/libts3client.so", "libts3client_linux_x86.so", "libts3client.so" };
                    break;
                case SupportedPlatform.Windows:
                    names = is64Bit
                        ? new string[] { "teamspeak_sdk_client.dll", Path.Combine(new string[] { "windows", "win64", "ts3client.dll" }), "ts3client_win64.dll", "ts3client.dll" }
                        : new string[] { "teamspeak_sdk_client.dll", Path.Combine(new string[] { "windows", "win32", "ts3client.dll" }), "ts3client_win32.dll", "ts3client.dll" };
                    break;
                case SupportedPlatform.iOS:
                    names = new string[] { "ts3client.framework/ts3client" };
                    break;
                default: throw new NotImplementedException();
            }

#if UNITY_EDITOR || UNITY_STANDALONE
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
        string subfolder = "Linux";
#elif UNITY_EDITOR_OSX
        string subfolder = "OSX";
#elif UNITY_STANDALONE_OSX
        string subfolder = null;
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            string subfolder = is64Bit ? "x86_64" : "x86";
#elif UNITY_IOS
        string subfolder = "iOS";
#elif UNITY_ANDROID
        string subfolder = "Android";
#else
        throw new NotImplementedException();
#endif
            string search_path = UnityEngine.Application.dataPath + "/Plugins/";
            if (!String.IsNullOrEmpty(subfolder))
            {
                search_path += subfolder + "/";
            }
            names = Array.ConvertAll(names, i => search_path + i);
#endif

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID
            // Resolve UPM Library/PackageCache entry for com.teamspeak.sdk
            string LibraryCache = "Library/PackageCache";
            try
            {
                LibraryCache = System.IO.Directory
                    .GetDirectories(LibraryCache)
                    .Where(dir => dir.Contains(PackageName))
                    .FirstOrDefault();
            } catch (System.IO.DirectoryNotFoundException) { /* nop */ }

            // extras: additional Unity search paths (UPM, AssetStore, Plugins, PackageCache)
            switch (platform)
            {
                case SupportedPlatform.Windows:
                    string[] add_names_win = is64Bit
                        ? new string[] { WindowsClientLibName,
                            string.Format("{0}/{1}/{2}", PackagePath, "windows/x86_64", WindowsClientLibName), // PkgManager
                            string.Format("{0}/{1}/{2}", AssetPath, "windows/x86_64", WindowsClientLibName), // Editor
                            string.Format("{0}/{1}/{2}", AssetStorePath, "windows/x86_64", WindowsClientLibName), // Asset Store
                            string.Format("{0}/{1}/{2}", LibraryCache, "Plugins/windows/x86_64", WindowsClientLibName) // PackageCache
#if UNITY_64
                            ,string.Format("{0}/{1}/{2}", UnityEngine.Application.dataPath, "Plugins", WindowsClientLibName)
                            ,string.Format("{0}/{1}/{2}/{3}", UnityEngine.Application.dataPath, "Plugins", "x86_64", WindowsClientLibName), string.Format("{0}/{1}/{2}", "Plugins", "x86_64", WindowsClientLibName)  // Standalone
#endif
#if UNITY_EDITOR
                            , Path.GetFullPath("Packages/com.teamspeak.sdk/Plugins/windows/x86_64/" + WindowsClientLibName)
#endif
                        }
                        : new string[] { WindowsClientLibName,
                            string.Format("{0}/{1}/{2}", PackagePath, "windows/x86", WindowsClientLibName), // PkgManager
                            string.Format("{0}/{1}/{2}", AssetPath, "windows/x86", WindowsClientLibName), // Editor
                            string.Format("{0}/{1}/{2}", AssetStorePath, "windows/x86", WindowsClientLibName), // Asset Store
                            string.Format("{0}/{1}/{2}", LibraryCache, "Plugins/windows/x86", WindowsClientLibName) // PackageCache
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                            ,string.Format("{0}/{1}/{2}", UnityEngine.Application.dataPath, "Plugins", WindowsClientLibName)
                            ,string.Format("{0}/{1}/{2}/{3}", UnityEngine.Application.dataPath, "Plugins", "x86", WindowsClientLibName), string.Format("{0}/{1}/{2}", "Plugins", "x86", WindowsClientLibName)  // Standalone
#endif
                        };
                    names = names.Concat(add_names_win).ToArray();
                    break;
                case SupportedPlatform.iOS:
                    names = new string[] { AppleClientLibName,
#if UNITY_64
                        string.Format("{0}/{1}", UnityEngine.Application.dataPath, AppleClientLibName), // Data
                        string.Format("{0}/../{1}/{2}", UnityEngine.Application.dataPath, "Frameworks", AppleClientLibName), // Frameworks
                        string.Format("{0}/../{1}/{2}", UnityEngine.Application.dataPath, "PlugIns", AppleClientLibName), // PlugIns
                        string.Format("{0}/../{1}/{2}", UnityEngine.Application.dataPath, "SharedSupport", AppleClientLibName) // SharedSupport
#endif
                    };
                    break;
                case SupportedPlatform.MacOSX:
                    string[] add_names =
                    {
                        string.Format("{0}/{1}/{2}", PackagePath, "macos/universal", AppleClientLibName), // PkgManager
                        string.Format("{0}/{1}/{2}", AssetPath, "macos/universal", AppleClientLibName), // Editor
                        string.Format("{0}/{1}/{2}", AssetStorePath, "macos/universal", AppleClientLibName), // Asset Store
                        string.Format("{0}/{1}/{2}", LibraryCache, "Plugins/macos/universal", AppleClientLibName) // PackageCache
#if UNITY_64
                        , string.Format("{0}/{1}/{2}", UnityEngine.Application.dataPath, "Plugins", AppleClientLibName)
                        , string.Format("{0}/{1}", "Plugins", AppleClientLibName) // Standalone appbundle
#endif
#if UNITY_EDITOR
                        , Path.GetFullPath("Packages/com.teamspeak.sdk/Plugins/macos/universal/" + AppleClientLibName)
#endif
                    };
                    names = names.Concat(add_names).ToArray();
                    break;
                case SupportedPlatform.Linux:
                    string[] add_names_linux = is64Bit
                        ? new string[] { LinuxClientLibName,
                            string.Format("{0}/{1}/{2}", PackagePath, "linux/x86_64", LinuxClientLibName), // PkgManager
                            string.Format("{0}/{1}/{2}", AssetPath, "linux/x86_64", LinuxClientLibName), // Editor
                            string.Format("{0}/{1}/{2}", AssetStorePath, "linux/x86_64", LinuxClientLibName), // Asset Store
                            string.Format("{0}/{1}/{2}", LibraryCache, "Plugins/linux/x86_64", LinuxClientLibName) // PackageCache
#if UNITY_64
                            ,string.Format("{0}/{1}/{2}", UnityEngine.Application.dataPath, "Plugins", LinuxClientLibName)
                            ,string.Format("{0}/{1}/{2}/{3}", UnityEngine.Application.dataPath, "Plugins", "x86_64", LinuxClientLibName), string.Format("{0}/{1}/{2}", "Plugins", "x86_64", LinuxClientLibName)  // Standalone
#endif
#if UNITY_EDITOR
                            , Path.GetFullPath("Packages/com.teamspeak.sdk/Plugins/linux/x86_64/" + LinuxClientLibName)
#endif
                        }
                        : new string[] { LinuxClientLibName };
                    names = names.Concat(add_names_linux).ToArray();
                    break;
                default: throw new NotImplementedException();
            }
#endif

            return true;
        }

        /// <summary>
        /// Returns the name of the native sdk binary that fits the current environment
        /// </summary>
        /// <param name="names">name of the native sdk binary</param>
        /// <param name="platform">detected platform</param>
        /// <returns>true if a matching binary exists</returns>
        public static bool TryGetNativeBinaryNameServer(out string[] names, out SupportedPlatform platform)
        {
            bool is64Bit;
            if (TryGetPlatform(out platform, out is64Bit) == false)
            {
                names = null;
                return false;
            }
            // get name of the binary
            switch (platform)
            {
                case SupportedPlatform.MacOSX:
                    names = new string[] { "libteamspeak_sdk_server.dylib", "mac/libts3server.dylib" };
                    break;
                case SupportedPlatform.Linux:
                    names = is64Bit
                        ? new string[] { "libteamspeak_sdk_server.so", "linux/amd64/libts3server.so" }
                        : new string[] { "libteamspeak_sdk_server.so", "linux/x86/libts3server_linux_x86.so" };
                    break;
                case SupportedPlatform.Windows:
                    names = is64Bit
                        ? new string[] { "teamspeak_sdk_server.dll", Path.Combine(new string[] { "windows", "win64", "ts3server.dll" }) }
                        : new string[] { "teamspeak_sdk_server.dll", Path.Combine(new string[] { "windows", "win32", "ts3server.dll" }) };
                    break;
                default: throw new NotImplementedException();
            }
            return true;
        }
    }
}
