using UnityEngine;
using UnityEngine.Audio;

namespace TwoCore
{
    [CreateAssetMenu(fileName = "GlobalConfig", menuName = "TwoCore/Global Config", order = 0)]
    public class GlobalConfig : SingletonScriptObject<GlobalConfig>
    {
        public int selectedProfileId = 0;
        public ProfileConfig SelectedProfile { get; set; }

        public bool VerboseLog => SelectedProfile ? SelectedProfile.verboseLog : true;
        public bool DebugLog => SelectedProfile ? SelectedProfile.debugLog : true;

        public bool resolutionScaling = true;
        [Tooltip("Apply scaling if device DPI is greater than this value.")]
        public int targetDPI = 360;
        public int screenSleepMode = SleepTimeout.NeverSleep;

        [Header("Language (Optional)")]
        public string defaultLanguage = "en";

        [Header("App Info")]
        public string gameId = "";

        [Header("Testing / Misc")]
        public bool simulateAutoUpdate = false;
        public bool removefbSDKtag = true;


        // --- Example enum for environment selection ---
        public LaunchEnvironment Environment => SelectedProfile ? SelectedProfile.environment : LaunchEnvironment.Dev;


        //--- Sound
        public AudioMixerGroup mainMixer;
        public string musicParam = "music";
        public string sfxParam = "sfx";
        public string ingameSfxParam = "ingame-sfx";
    }

    public enum LaunchEnvironment
    {
        Local,
        Dev,
        Live,
        Staging,
        LocalNetwork
    }

#if !UNITY_ANDROID
    public enum AppUpdateType
    {
        Flexible = 0,
        Immediate = 1
    }
#endif
}