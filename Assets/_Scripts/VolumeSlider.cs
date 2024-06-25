using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider musicSlider; // Referencia al Slider en el Inspector

    void Start()
    {
        // Configura el valor inicial del Slider basado en el volumen actual del AudioManager
        musicSlider.value = AudioManager.Instance.GetMusicVolume();

        // Escucha los cambios en el Slider
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
    }

    void OnMusicSliderValueChanged(float value)
    {
        // Ajusta el volumen de la música en el AudioManager basado en el valor del Slider
        AudioManager.Instance.SetMusicVolume(value);
    }
}
