using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidID = "5646125";
    [SerializeField] bool _isTesting;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_androidID, _isTesting, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ads Initialized...");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"{error}, {message}");
    }
}
