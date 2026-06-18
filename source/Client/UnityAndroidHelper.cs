using System;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace TeamSpeak.Sdk.Client.Unity
{

    public static class UnityAndroidHelper
    {
        // Returns the system architecture
        public static string GetArchitecture()
        {
            using (var system = new AndroidJavaClass("java.lang.System"))
            {
                return system.CallStatic<string>("getProperty", "os.arch");
            }
        }

        public static string[] GetSupportedAbis()
        {
            AndroidJavaClass jc = new AndroidJavaClass("android.os.Build");
            // TODO? this is api level 21+, if necessary look into CPU_ABI, CPI_ABI2
            return jc.GetStatic<string[]>("SUPPORTED_ABIS");
        }

        public static string GetSupportedAbi()
        {
            string[] teamspeakAbis = new string[] { "armeabi-v7a", "arm64-v8a", "x86", "x86_64" };
            string[] supportedAbis = GetSupportedAbis();
            string[] res = Array.FindAll(supportedAbis, f => Array.FindIndex(teamspeakAbis, g => g == f) != -1);
            if (res.Length == 0)
            {
                return null;
            }
            return res[0];
        }

        public static string getNativeLibraryDir()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject appInfo = context.Call<AndroidJavaObject>("getApplicationInfo");
            return appInfo.Get<string>("nativeLibraryDir");
        }
    }
}
#endif
