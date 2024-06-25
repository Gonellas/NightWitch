using UnityEngine;
using UnityEngine.UI;

public class SfxSlider : MonoBehaviour
{

    public Slider sfxSlider; 

    void Start()
    {
        sfxSlider.value = AudioManager.Instance.GetMusicVolume();

        sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
    }

    void OnSfxSliderValueChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }
}

