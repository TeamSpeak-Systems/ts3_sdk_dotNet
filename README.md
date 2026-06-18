TeamSpeak SDK .NET
==================

A C# wrapper around the native TeamSpeak SDK. It exposes the SDK's functionality
through a fluent, object-oriented and async API, for both the **client** and the
**server** side.

Targets the TeamSpeak SDK **3.5.2**. The native libraries are not committed to this
repository — they are fetched by the bootstrap tool (see *Getting the native SDK*).

Main features
-------------

* Client and server wrappers over the native TeamSpeak SDK
* Modern, object-oriented API with `async`/`Task`-based connection handling
* Multi-targeted: `net8.0`, `net45`, `netstandard2.0`

Target frameworks
-----------------

The wrapper (`source/TeamSpeak.Sdk.csproj`) builds for `netstandard2.0;net45;net8.0`.
For modern .NET applications use `net8.0` (the minimal in-support LTS). `net45` and
`netstandard2.0` remain for .NET Framework / Mono / Unity consumers.

Getting the native SDK
----------------------

The native client/server libraries are downloaded and deployed by the bootstrap tool
rather than checked in. The version is pinned in `tools/bootstrap_config.json`:

```json
{
  "origin": { "url": "https://files.teamspeak-services.com/releases/sdk", "version": "3.5.2" },
  "platforms": ["windows-x86_64"]
}
```

Run it once (requires only the .NET SDK):

```
dotnet run --project tools/Bootstrap
```

This downloads `teamspeak-sdk-<version>.tar.gz` and deploys the client and server
native libraries into the gitignored `external/teamspeak-sdk/<platform>/`. The example
projects copy those libraries to their build output and load them from there, so once
the bootstrap has run the examples build and run as-is. Add more platforms (e.g.
`linux-x86_64`, `macos-armv8`) to the `platforms` list as needed.

Main classes
------------

Client (`TeamSpeak.Sdk.Client`):

* `Library` — the native clientlib
* `Connection` — a connection to a SDK server
* `Client` — a client on a SDK server
* `Channel` — a channel on a SDK server
* `FileTransfer` — a file transfer to or from a SDK server
* `WaveHandle` — a wave file played back locally

Server (`TeamSpeak.Sdk.Server`):

* `Library` — the native serverlib
* `VirtualServer` — a virtual server hosted by this process
* `Channel` / `Client` — channels and clients on a hosted virtual server

Client usage
------------

If the native clientlib is on the library search path or next to the wrapper assembly,
the SDK initializes automatically on first use:

```csharp
using TeamSpeak.Sdk.Client;
// ...
using (Connection connection = new Connection())
{
    string identity = Library.CreateIdentity();
    await connection.Start(identity, "localhost", 9987, "client");
    await connection.SendTextMessage("Hello, World!");
    await connection.Stop("And good bye!");
}
```

Initialization can be forced at any time with `Library.Initialize`. Encryption, logging
and the native-library location are configured through `LibraryParameters`:

```csharp
using TeamSpeak.Sdk.Client;
// ...
LibraryParameters parameters = new LibraryParameters(pathToNativeLibraryFolder)
{
    UsedLogTypes = LogTypes.File | LogTypes.Console,
    LogFileFolder = "/var/log/teamspeak/",
    // Optional custom handlers; leave unassigned for default handling:
    // ClientPasswordEncrypt = EncryptClientPassword,
    // CustomPacketEncrypt = EncryptPacket,
    // CustomPacketDecrypt = DecryptPacket,
};
try
{
    Library.Initialize(parameters);
}
catch (DllNotFoundException)
{
    // handle missing native library
}
```

The example projects pass `AppContext.BaseDirectory` as the native-library folder, since
the bootstrapped libraries are copied next to the executable.

Server usage
------------

The same package wraps the native serverlib under `TeamSpeak.Sdk.Server`. A minimal
in-process virtual server:

```csharp
using TeamSpeak.Sdk.Server;
// ...
Library.Initialize(new LibraryParameters(pathToNativeLibraryFolder));
VirtualServer server = Library.CreateVirtualServer(
    maxClients: 8, keyPair: "", port: 9987, ip: "0.0.0.0", name: "My SDK Server");
server.Password = "secret";
```

Examples
--------

Located in `examples/` (all `net8.0`):

* `Client` — full-featured client sample
* `ClientMinimal` — smallest client that connects and exchanges events
* `Server` — hosts a virtual server with an interactive console

Run the bootstrap first, then e.g. `dotnet run --project examples/ClientMinimal`.

Mobile (Android / iOS)
----------------------

Mobile is **not currently a supported target** of this wrapper. Some Unity-Android
binding code (`TeamSpeak.Sdk.Client.Unity`) and legacy Xamarin sample projects under
`examples/Android` and `examples/iOS` remain in the tree, but they predate the current
`net8.0` layout, are kept for reference only, and are not covered by the bootstrap/examples
workflow above. First-class mobile support, if pursued, would be a separate future effort.
