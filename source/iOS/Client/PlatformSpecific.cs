using System;
using System.Runtime.InteropServices;
using TeamSpeak.Sdk;

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

	public static void LoadDynamicLibrary(SupportedPlatform platform, string fileName, out IntPtr handle, out string location)
	{
		if (platform != SupportedPlatform.iOS) throw new NotSupportedException();
		handle = NativeUnixMehods.dlopen(fileName, 2 /* RTLD_NOW */);
		if (handle == IntPtr.Zero)
			throw new DllNotFoundException(fileName ?? "<static linked>", GetLastError());
		location = fileName;
	}

	public static void GetLibraryMethod<T>(SupportedPlatform platform, IntPtr handle, string name, out T t)
	{
		if (platform != SupportedPlatform.iOS) throw new NotSupportedException();
		IntPtr result = NativeUnixMehods.dlsym(handle, name);
		if (result == IntPtr.Zero)
			throw new EntryPointNotFoundException(name, GetLastError());
		t = (T)(object)Marshal.GetDelegateForFunctionPointer(result, typeof(T));
	}

	public static void UnloadDynamicLibrary(SupportedPlatform platform, IntPtr handle)
	{
		if (platform != SupportedPlatform.iOS) throw new NotSupportedException();
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
	/// <param name="name">name of the native sdk binary</param>
	/// <param name="platform">detected platform</param>
	/// <returns>true if a matching binary exists</returns>
	public static bool TryGetNativeBinaryName(out string name, out SupportedPlatform platform)
	{
		platform = SupportedPlatform.iOS;
		name = null;
		return true;
	}
}