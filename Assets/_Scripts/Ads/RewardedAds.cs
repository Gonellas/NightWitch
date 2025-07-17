using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _rewardedID = "Rewarded_Android";

    public static bool isEnergyAd;

    public void LoadRewardedAd()
    {
        Advertisement.Load(_rewardedID, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(_rewardedID, this);
        LoadRewardedAd();
    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded_Android_Ad loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Rewarded_Android_Ad failed");
    }


    public void OnUnityAdsShowClick(string placementId)
    { }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Rewarded_Android_Ad Start");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {

        if (placementId == _rewardedID)
        {
            Debug.Log("me da algo");
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                if (isEnergyAd)
                {
                    GameManager.instance.GiveEnergy(1);
                }
                else
                {
                    GameManager.instance.GiveCurrency(10);
                }
            }

            else if (showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
            {
                if (isEnergyAd)
                {
                    GameManager.instance.GiveEnergy(1);
                }
                else
                {
                    GameManager.instance.GiveCurrency(10);
                }
            }

            else if (showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN))
                Debug.Log("Something is wrong");
        }

       else
        {
            Debug.Log("no me da nada");
        }
    }
}
