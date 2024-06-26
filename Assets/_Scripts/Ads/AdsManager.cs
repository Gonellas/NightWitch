using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    public RewardedAds _rewardedAds;
    public IntestitialAds _intestitialAds;
    public BannerAd _bannerAd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        _rewardedAds.LoadRewardedAd();
        StartCoroutine(InterstitialAd());
        StartCoroutine(BannerAd());
    }

    // Start is called before the first frame update
    public void ShowRewarded()
    {
        _rewardedAds.ShowRewardedAd();
    }


    IEnumerator InterstitialAd()
    {
        _intestitialAds.LoadInterstitialAd();
        yield return new WaitForSeconds(5f);
        _intestitialAds.ShowInterstitialAd();
    }

    IEnumerator BannerAd()
    {
        while (true)
        {
            _bannerAd.LoadBannerAd();
            yield return new WaitForSeconds(5);
            _bannerAd.ShowBannerAd();
            yield return new WaitForSeconds(30);
            _bannerAd.HideBannerAd();
            yield return new WaitForSeconds(30);
        }
    }
}
