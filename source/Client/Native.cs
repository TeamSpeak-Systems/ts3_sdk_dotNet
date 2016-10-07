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

internal static class Native
{
    public const System.Runtime.InteropServices.CallingConvention CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;
    public static readonly Encoding Encoding = Encoding.UTF8;
    public static readonly int SizeOfPointer = Marshal.SizeOf(typeof(IntPtr));

    public static string ReadString(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero) return null;
        int length = 0;
        while (Marshal.ReadByte(pointer, length) != 0) length += 1; ;
        byte[] bytes = new byte[length];
        Marshal.Copy(pointer, bytes, 0, length);
        return Encoding.GetString(bytes);
    }

    public static List<ushort> ReadAndFreeUInt16List(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero) return null;
        List<ushort> result = new List<ushort>();
        for (int offset = 0; ; offset += sizeof(ushort))
        {
            ushort item = (ushort)Marshal.ReadInt16(pointer, offset);
            if (item == 0) break;
            result.Add(item);
        }
        Library.Api.FreeMemory(pointer);
        return result;
    }

    public static List<ulong> ReadAndFreeUInt64List(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero) return null;
        List<ulong> result = new List<ulong>();
        for (int offset = 0; ; offset += sizeof(ulong))
        {
            ulong item = (ulong)Marshal.ReadInt64(pointer, offset);
            if (item == 0) break;
            result.Add(item);
        }
        Library.Api.FreeMemory(pointer);
        return result;
    }

    public static List<T> ReadAndFreeShortIDList<T>(IntPtr pointer, Func<ushort, T> createFunc)
    {
        if (pointer == IntPtr.Zero) return null;
        const int sizeOfItem = 2;
        List<T> result = new List<T>();
        for (int offset = 0; ; offset += sizeOfItem)
        {
            ushort id = (ushort)Marshal.ReadInt16(pointer, offset);
            if (id == 0) break;
            result.Add(createFunc(id));
        }
        Library.Api.FreeMemory(pointer);
        return result;
    }
    public static List<T> ReadAndFreeLongIDList<T>(IntPtr pointer, Func<ulong, T> createFunc)
    {
        if (pointer == IntPtr.Zero) return null;
        const int sizeOfItem = 8;
        List<T> result = new List<T>();
        for (int offset = 0; ; offset += sizeOfItem)
        {
            ulong id = (ulong)Marshal.ReadInt64(pointer, offset);
            if (id == 0) break;
            result.Add(createFunc(id));
        }
        Library.Api.FreeMemory(pointer);
        return result;
    }

    public static List<T> ReadAndFreePointerList<T>(IntPtr pointer, Func<IntPtr, T> readAndFreeFunc)
    {
        if (pointer == IntPtr.Zero) return null;
        List<T> result = new List<T>();
        for (int offset = 0; ; offset += SizeOfPointer)
        {
            IntPtr itemPointer = Marshal.ReadIntPtr(pointer, offset);
            if (itemPointer == IntPtr.Zero) break;
            result.Add(readAndFreeFunc(itemPointer));
        }
        Library.Api.FreeMemory(pointer);
        return result;
    }

    public static string ReadAndFreeString(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero) return null;
        string result = ReadString(pointer);
        Library.Api.FreeMemory(pointer);
        return result;
    }

    public static Func<IntPtr, SoundDevice> ReadAndFreeSoundDevice(string mode)
    {
        return (pointer) =>
        {
            IntPtr pName = Marshal.ReadIntPtr(pointer, 0);
            IntPtr pID = Marshal.ReadIntPtr(pointer, SizeOfPointer);
            SoundDevice result = new SoundDevice(mode, Native.ReadString(pID), Native.ReadString(pName));
            Library.Api.FreeMemory(pID);
            Library.Api.FreeMemory(pName);
            Library.Api.FreeMemory(pointer);
            return result;
        };
    }

    public static UnmanagedPointer WriteArray(ushort[] values)
    {
        IntPtr result;
        if (values != null)
        {
            const int sizeOfItem = 2;
            result = Marshal.AllocHGlobal((values.Length + 1) * sizeOfItem);
            Marshal.Copy((short[])(object)values, 0, result, values.Length);
            Marshal.WriteInt16(result, values.Length * sizeOfItem, 0);
        }
        else result = IntPtr.Zero;
        return new UnmanagedPointer(result);
    }

    public static UnmanagedPointer WriteArray(ulong[] values)
    {
        IntPtr result;
        if (values != null)
        {
            const int sizeOfItem = 8;
            result = Marshal.AllocHGlobal((values.Length + 1) * sizeOfItem);
            Marshal.Copy((long[])(object)values, 0, result, values.Length);
            Marshal.WriteInt64(result, values.Length * sizeOfItem, 0);
        }
        else result = IntPtr.Zero;
        return new UnmanagedPointer(result);
    }

    public static UnmanagedPointer WriteShortIDArray<T>(T[] items, Func<T, ushort> getID)
    {
        IntPtr result;
        if (items != null)
        {
            const int sizeOfItem = 2;
            result = Marshal.AllocHGlobal((items.Length + 1) * sizeOfItem);
            for (int i = 0; i < items.Length; i++)
            {
                Marshal.WriteInt16(result, i * sizeOfItem, (short)getID(items[i]));
            }
            Marshal.WriteInt16(result, items.Length * sizeOfItem, 0);
        }
        else result = IntPtr.Zero;
        return new UnmanagedPointer(result);
    }

    public static UnmanagedPointer WriteLongIDArray<T>(T[] items, Func<T, ulong> getID)
    {
        IntPtr result;
        if (items != null)
        {
            const int sizeOfItem = 8;
            result = Marshal.AllocHGlobal((items.Length + 1) * sizeOfItem);
            for (int i = 0; i < items.Length; i++)
            {
                Marshal.WriteInt64(result, i * sizeOfItem, (long)getID(items[i]));
            }
            Marshal.WriteInt64(result, items.Length * sizeOfItem, 0);
        }
        else result = IntPtr.Zero;
        return new UnmanagedPointer(result);
    }

    public static UnmanagedPointer WriteStringArray(string[] values)
    {
        IntPtr result;
        if (values != null)
        {
            int sizeOfArray = (values.Length + 1) * SizeOfPointer;
            int sizeOfItems = 0;
            int[] sizes = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                sizes[i] = Encoding.GetByteCount(values[i]) + 1;
                sizeOfItems += sizes[i];
            }
            result = Marshal.AllocHGlobal(sizeOfArray + sizeOfItems);
            IntPtr arrayPointer = result;
            IntPtr dataPointer = result + sizeOfArray;
            for (int i = 0; i < values.Length; i++)
            {
                int size = sizes[i];
                string value = values[i];
                Marshal.WriteIntPtr(arrayPointer, dataPointer);
                WriteString(value, dataPointer, size);
                arrayPointer += SizeOfPointer;
                dataPointer += size;
            }
            Marshal.WriteIntPtr(arrayPointer, IntPtr.Zero);
        }
        else
        {
            result = IntPtr.Zero;
        }
        return new UnmanagedPointer(result);
    }

    public static UnmanagedPointer WriteString(string text)
    {
        IntPtr result;
        if (text != null)
        {
            byte[] bytes = Encoding.GetBytes(text);
            result = Marshal.AllocHGlobal(bytes.Length + 1);
            Marshal.Copy(bytes, 0, result, bytes.Length);
            Marshal.WriteByte(result, bytes.Length, 0);
        }
        else result = IntPtr.Zero;
        return new UnmanagedPointer(result);
    }

    public static int WriteString(string text, IntPtr pointer, int size)
    {
        byte[] bytes = new byte[size];
        int n = Encoding.GetBytes(text, 0, text.Length, bytes, 0);
        Marshal.Copy(bytes, 0, pointer, n);
        Marshal.WriteByte(pointer, n, 0);
        return n;
    }

    public static void FreeUnmanaged(IntPtr pointer)
    {
        Marshal.FreeHGlobal(pointer);
    }
    public static void FreeUnmanagedPointerArray(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero) return;
        for (int offset = 0; ; offset += SizeOfPointer)
        {
            IntPtr itemPointer = Marshal.ReadIntPtr(pointer, offset);
            if (itemPointer == IntPtr.Zero) break;
            Marshal.FreeHGlobal(itemPointer);
        }
        Marshal.FreeHGlobal(pointer);
    }

    static DateTimeOffset Epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

    public static DateTimeOffset FromUnixTime(ulong datetime)
    {
        return Epoch.AddSeconds(datetime).ToLocalTime();
    }
}

internal class UnmanagedPointer : SafeHandleZeroOrMinusOneIsInvalid
{
    public UnmanagedPointer(IntPtr pointer)
        : base(true)
    {
        SetHandle(pointer);
    }

    protected override bool ReleaseHandle()
    {
        Native.FreeUnmanaged(handle);
        return true;
    }
}
