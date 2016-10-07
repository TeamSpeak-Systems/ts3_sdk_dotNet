using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamSpeak.Sdk.Client;

internal static class Require
{
    public static void NotNull<T>(string name, T t)
        where T : class
    {
        if (t == null) throw new ArgumentNullException(name);
    }

    public static void EntriesNotNull<T>(string name, T[] array)
    {
        if (array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null) throw new ArgumentNullException(GetIndexedName(name, i));
            }
        }
    }

    public static void SameConnection(string name, Connection connection, Channel channel)
    {
        if (channel != null && channel.Connection != connection)
            throw new ArgumentException("Channel is bound to another ServerConnection.", name);
    }

    public static void SameConnection(string name, Connection connection, Channel[] channelArray)
    {
        if (channelArray != null)
        {
            for (int i = 0; i < channelArray.Length; i++)
            {
                if (channelArray[i] != null && channelArray[i].Connection != connection)
                    throw new ArgumentException("Channel is bound to another ServerConnection.", GetIndexedName(name, i));
            }
        }
    }
    public static void SameConnection(string name, Connection connection, Client client)
    {
        if (client != null && client.Connection != connection)
            throw new ArgumentException("Client is bound to another ServerConnection.", name);
    }

    public static void SameConnection(string name, Connection connection, Client[] clientArray)
    {
        if (clientArray != null)
        {
            for (int i = 0; i < clientArray.Length; i++)
            {
                if (clientArray[i] != null && clientArray[i].Connection != connection)
                    throw new ArgumentException("Client is bound to another ServerConnection.", GetIndexedName(name, i));
            }
        }
    }

    public static void NotNullOrEmpty(string name, string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException("must not be null or empty", name);
    }
    public static void NotNullOrEmpty<T>(string name, T[] array)
    {
        if (array == null || array.Length == 0)
            throw new ArgumentNullException("must not be null or empty", name);
    }

    internal static void ValidRange<T>(string nameArray, string nameOffset, string nameCount, T[] buffer, int offset, int count)
    {
        if (buffer != null)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameOffset, nameOffset + " must not be negative.");
            }
            else if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameCount, nameCount + " must not be negative.");
            }
            else if (offset + count > buffer.Length)
            {
                string message = $"{nameOffset} and {nameCount} were out of bounds for the array or {nameCount} is greater than the number of elements from {nameOffset}  to the end of {nameArray}.";
                throw new ArgumentOutOfRangeException(message);
            }
        }
    }

    internal static void ValidRange<T>(string nameArray, string nameCount, T[] buffer, int count)
    {
        if (buffer != null)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameCount, nameCount + " must not be negative.");
            }
            else if (count > buffer.Length)
            {
                string message = $"{nameCount} is greater than the number of elements of {nameArray}.";
                throw new ArgumentOutOfRangeException(message);
            }
        }
    }

    private static string GetIndexedName(string name, int index)
    {
        return name + '[' + index + ']';
    }
}
