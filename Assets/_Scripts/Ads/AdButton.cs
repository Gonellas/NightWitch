using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdButton : MonoBehaviour
{
    public void ExecuteAd() => AdsManager.Instance.ShowRewarded();
}
