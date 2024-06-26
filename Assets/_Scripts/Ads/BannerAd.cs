using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    [SerializeField] string _bannerID = "Banner_Android";

    private void Awake()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };

        Advertisement.Banner.Load(_bannerID,options);
    }

    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHidden
        };
        Advertisement.Banner.Show(_bannerID, options);

    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    private void BannerHidden() { }

    private void BannerClicked() { }

    private void BannerShown() { }

    private void BannerLoadedError(string message) { }

    private void BannerLoaded()
    {
        Debug.Log("Banner Ad Loaded");
    }
}
