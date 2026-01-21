using System;
using System.Collections.Generic;
using Hawky.Inventory;
#if HAS_LION_APPLOVIN_SDK
using LionStudios.Suite.Analytics;
#endif
namespace Hawky.Ads
{
    public class ShowInterstitialRequest
    {
        public string position = ShowInterstitialPositionId.DEFAULT;
        public string placementId;
    }

    public class ShowRewardRequest
    {
        public string position = ShowInterstitialPositionId.DEFAULT;
        public string placementId;
        public string featureName;
        public int levelId;
        public string missionType;
        public string missionName;
        public int missionId;
        public int missionAttempt;
        public List<ItemData> rewards = new List<ItemData>();
#if HAS_LION_APPLOVIN_SDK
        public Reward? reward = null;
#endif

        public ShowRewardRequest()
        {
            placementId = string.Empty;
            featureName = string.Empty;
        }

        public ShowRewardRequest(string placementId = "UNKNOWN", string featureName = "UNKNOWN")
        {
            this.placementId = placementId;
            this.featureName = featureName;
        }

        public ShowRewardRequest(string placementId = "UNKNOWN", int levelId = 0, string featureName = "UNKNOWN", string missionType = "UNKNOWN", string missionName = "UNKNOWN", int missionId = 0, int missionAttempt = 0)
        {
            this.placementId = placementId;
            this.featureName = featureName;
            this.levelId = levelId;
            this.missionType = missionType;
            this.missionName = missionName;
            this.missionId = missionId;
            this.missionAttempt = missionAttempt;
        }
    }

    public class ShowBannerRequest
    {
        public bool value;
    }

    public partial class ShowInterstitialPositionId
    {
        public const string DEFAULT = "Unknown";
    }

    public partial class ShowRewardPositionId
    {
        public const string DEFAULT = "Unknown";
    }

    public interface IShowInterstitialAdsHandler
    {
        void ShowInterstitial(ShowInterstitialRequest request, Action<ShowAdsResult> callBack);
        float Interval();
    }

    public interface IShowRewardAdsHandler
    {
        void ShowReward(ShowRewardRequest request, Action<ShowAdsResult> callBack);
    }

    public interface IRewardAdActionHandler
    {
        void ShowNoAdAvailable();
        bool NoShowAdBasedOnConfig(string featureName);
    }

    public interface IShowBannerAdsHanlder
    {
        void ShowBanner(ShowBannerRequest request, Action<bool> callback);
    }

    public class AdsResultId
    {
        public const int RESULT_INPROGRESS = 0;
        public const int RESULT_OK = 1;
        public const int RESULT_BACK = 2;
    }

    public interface IBannerRegister
    {

    }

    public interface ILockBannerRegister
    {

    }
}