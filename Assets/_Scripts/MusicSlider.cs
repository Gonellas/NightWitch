using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    public Slider musicSlider; 

    void Start()
    {
        musicSlider.value = AudioManager.Instance.GetMusicVolume();

        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
    }

    void OnMusicSliderValueChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }
}
