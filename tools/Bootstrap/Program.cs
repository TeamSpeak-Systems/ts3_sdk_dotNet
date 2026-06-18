using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

// Downloads the TeamSpeak SDK release archive named in tools/bootstrap_config.json and
// deploys the native client/server libraries for the configured platforms into a
// gitignored folder (external/teamspeak-sdk/<platform>/), giving examples and the
// validation harness a stable path to load the native binaries from.
//
// Run from anywhere:  dotnet run --project tools/Bootstrap
internal static class Program
{
    // Absolute path of this source file at build time; mirrors bootstrap.py's use of __file__.
    private static string SourceFile([CallerFilePath] string path = "") => path;

    private static async Task<int> Main()
    {
        string toolsDir = Directory.GetParent(Path.GetDirectoryName(SourceFile())!)!.FullName; // .../tools
        string repoRoot = Directory.GetParent(toolsDir)!.FullName;                             // .../public
        string configPath = Path.Combine(toolsDir, "bootstrap_config.json");

        using JsonDocument config = JsonDocument.Parse(File.ReadAllText(configPath));
        JsonElement origin = config.RootElement.GetProperty("origin");
        string baseUrl = origin.GetProperty("url").GetString()!.TrimEnd('/');
        string version = origin.GetProperty("version").GetString()!;

        List<string> platforms = new();
        foreach (JsonElement p in config.RootElement.GetProperty("platforms").EnumerateArray())
            platforms.Add(p.GetString()!);

        string url = $"{baseUrl}/{version}/teamspeak-sdk-{version}.tar.gz";
        string destRoot = Path.Combine(repoRoot, "external", "teamspeak-sdk");

        string archivePath = Path.Combine(Path.GetTempPath(), $"teamspeak-sdk-{version}-{Guid.NewGuid():N}.tar.gz");
        string extractDir = Path.Combine(Path.GetTempPath(), $"teamspeak-sdk-{version}-{Guid.NewGuid():N}");
        try
        {
            Console.WriteLine($"downloading {url}");
            using (HttpClient http = new())
            using (HttpResponseMessage resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                resp.EnsureSuccessStatusCode();
                await using FileStream fs = File.Create(archivePath);
                await resp.Content.CopyToAsync(fs);
            }

            Console.WriteLine("extracting...");
            Directory.CreateDirectory(extractDir);
            await using (FileStream gzFile = File.OpenRead(archivePath))
            using (GZipStream gz = new(gzFile, CompressionMode.Decompress))
                TarFile.ExtractToDirectory(gz, extractDir, overwriteFiles: true);

            string archiveRoot = Path.Combine(extractDir, $"teamspeak-sdk-{version}");
            foreach (string platform in platforms)
            {
                string dst = Path.Combine(destRoot, platform);
                if (Directory.Exists(dst))
                    Directory.Delete(dst, recursive: true);
                Directory.CreateDirectory(dst);

                bool deployed = false;
                foreach (string kind in new[] { "client_sdk", "server_sdk" })
                {
                    string binDir = Path.Combine(archiveRoot, kind, platform, "bin");
                    if (!Directory.Exists(binDir))
                        continue;
                    foreach (string file in Directory.EnumerateFiles(binDir))
                    {
                        File.Copy(file, Path.Combine(dst, Path.GetFileName(file)), overwrite: true);
                        deployed = true;
                    }
                }

                Console.WriteLine(deployed
                    ? $"  deployed {platform} -> {dst}"
                    : $"  WARN: {platform} not found in archive (no client_sdk/server_sdk bin) — skipping");
            }
        }
        finally
        {
            try { File.Delete(archivePath); } catch { /* best effort */ }
            try { if (Directory.Exists(extractDir)) Directory.Delete(extractDir, recursive: true); } catch { /* best effort */ }
        }

        Console.WriteLine("done");
        return 0;
    }
}
