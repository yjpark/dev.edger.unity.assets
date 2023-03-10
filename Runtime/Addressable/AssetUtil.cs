using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public static class AssetsUtil {
        public static string _BuildTarget = null;
        public static string BuildTarget {
            get {
                if (_BuildTarget == null) {
                    _BuildTarget = CalcBuildTarget(Application.platform);
                }
                return _BuildTarget;
            }
        }

        public static string CalcBuildTarget(RuntimePlatform platform) {
            // Unity's BuildTarget is in UnityEditor namespace.
            // https://docs.unity3d.com/ScriptReference/RuntimePlatform.html
            // https://docs.unity3d.com/ScriptReference/BuildTarget.html
            switch (platform) {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.PS4:
                    return "PS4";
                case RuntimePlatform.XboxOne:
                    return "XboxOne";
                case RuntimePlatform.tvOS:
                    return "tvOS";
                case RuntimePlatform.Switch:
                    return "Switch";
                case RuntimePlatform.Stadia:
                    return "Stadia";
                case RuntimePlatform.PS5:
                    return "PS5";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsServer:
                    if (IntPtr.Size == 8) {
                        return "StandaloneWindows64";
                    } else {
                        return "StandaloneWindows";
                    }
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXServer:
                    return "StandaloneOSX";
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxServer:
                    return "StandaloneLinux64";
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerARM:
                    return "WSAPlayer";
            }
            return "";
        }
    }
}
