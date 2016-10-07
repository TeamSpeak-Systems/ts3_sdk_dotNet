TeamSpeak SDK .Net
==================

A C#-Wrapper around the native TeamSpeak-SDK. It provides the same functionality
in a more fluent, object-oriented and asynchron API. In order to function the
TeamSpeak 3 SDK 3.0.3.2 (http://TeamSpeak.com/downloads#sdk) must be placed in
root of the Project.

Main features:

* Cross-platform (Windows, Linux, MacOS, Android, iOS)
* Fully-featured TeamSpeak-SDK
* Modern API with object-oriented design 

Main Classes
------------

* TeamSpeak.SDK.Client.Library - native clientlib
* TeamSpeak.SDK.Client.Connection - connection to a SDK server
* TeamSpeak.SDK.Client.Client - client on a SDK server 
* TeamSpeak.SDK.Client.Channel - channel on a SDK server
* TeamSpeak.SDK.Client.FileTransfer - file transfer from or to a SDK server
* TeamSpeak.SDK.Client.WaveHandler - wave file playing locally

Usage in Linux, MacOS, Windows
------------------------------

If the native clientlib is in the library search path or next to the sdk 
assembly, initialising of the SDK is done automaticlly in the background.
```
using TeamSpeak.SDK.Client;
/// ...
using (Connection connection = new Connection())
{
	await connection.Start(identity, "localhost", 9987, "client");
	await connection.SendTextMessage("Hello, World!");
	await connection.Stop("And good bye!");
}
```

Initialization of the SDK can be forced at any time using `Library.Initialize`.
```
using TeamSpeak.SDK.Client;
/// ...
try
{
	Library.Initialize()
}
catch (DllNotFoundException)
{
  // handle missing native library
}
using (Connection connection = new Connection())
{
	// ...
}
``` 

If the native library is not in the search path, it must be specified 
using TeamSpeak.SDK.Client;
```
using TeamSpeak.SDK.Client;
/// ...
try
{
	LibraryParameters parameters = new LibraryParameters("/tmp/renamed_libts3client_linux_amd64.so");
	Library.Initialize(parameters);
}
catch (DllNotFoundException)
{
  // handle missing native library
}
/// ...
```

Encryption and logging must be configured when the library is initialized.
This is also done in `LibraryParameters`.
```
LibraryParameters parameters = new LibraryParameters()
{
	// where the logs are written to
	LogFileFolder = "/var/log/teamspeak/",
	// Custom password encryption, leave unassigned for default handling
	ClientPasswordEncrypt = EncryptClientPassword,
	// Custom password encryption, leave unassigned for default handling
	CustomPacketEncrypt = EncryptPacket,
	// Custom package decryption, leave unassigned for default handling
	CustomPacketDecrypt = DecryptPacket,
}
try
{
	Library.Initialize(parameters);
}
catch (DllNotFoundException)
{
  // handle missing native library
}
```

Usage in Android
----------------

The SDK contains a Xamarin poject for Android, it can be found 
in `Source/Android`. The Library only needs to be referenced to be used.
It already contains the Android specific native libraries.

```
using TeamSpeak.SDK.Client;
/// ...
using (Connection connection = new Connection())
{
	await connection.Start(identity, "localhost", 9987, "client");
	await connection.SendTextMessage("Hello, World!");
	await connection.Stop("And good bye!");
}
```

Usage in iOS
------------

A Xamarin project for iOS is located at `Source/iOS`. In order for the sdk to
work it is important that the correct static library is linked when building the
app. For i386 and x86_64 `ts3_sdk_3.0.3.2/iOS/
libts3client_ios_sdk_simulator.a` needs to be linked. For ARMv7, ARMv7s and ARM64
instead link against `ts3_sdk_3.0.3.2/iOS/libts3client_ios_sdk_device.a`.
Please see the example project in `Examples\iOS` for more details.

Aside from statically linking the native library, the TeamSpeak SDK is identical
to the other platforms.

```
using TeamSpeak.SDK.Client;
/// ...
using (Connection connection = new Connection())
{
	await connection.Start(identity, "localhost", 9987, "client");
	await connection.SendTextMessage("Hello, World!");
	await connection.Stop("And good bye!");
}
```

