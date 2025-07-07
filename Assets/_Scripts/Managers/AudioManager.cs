using System;
using UnityEngine;

public enum SoundType
{
    MainTheme_1,
    MainTheme_2,
    Player_MovementR,
    Player_MovementL,
    Zombie_MovementR,
    Zombie_MovementL,
    Click,
    Rawr,
    Coin,
    Explosion,
    Thunder,
    Ice,
    Ground,
    Fire,
    Shield
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Values")]
    [SerializeField] private float _musicVolume = 1.0f;
    [SerializeField] private float _sfxVolume = 1.0f;

    private AudioSource audioSource;
    private AudioSource audioSource2;
    private AudioSource sfxSource;
    private SoundType _soundType;

    [SerializeField] private AudioClip[] _soundList;

    [SerializeField] private bool _firstAudioSourceIsPlaying;

    public SoundType SoundType => _soundType;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set { instance = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource2 = this.gameObject.AddComponent<AudioSource>();
            sfxSource = this.gameObject.AddComponent<AudioSource>();

            audioSource.loop = true;
            audioSource2.loop = true;

            
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

            
            audioSource.volume = _musicVolume;
            audioSource2.volume = _musicVolume;
            sfxSource.volume = _sfxVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(SoundType.MainTheme_1, 1);
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);

        audioSource.volume = _musicVolume;
        audioSource2.volume = _musicVolume;

        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        PlayerPrefs.Save();
    }

    public float GetSFXVolume()
    {
        return _sfxVolume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = _sfxVolume;

        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        PlayerPrefs.Save();
    }

    public void PlayMusic(SoundType soundType, float volume)
    {
        AudioSource activeSource = _firstAudioSourceIsPlaying ? audioSource : audioSource2;

        activeSource.clip = _soundList[(int)soundType];
        activeSource.volume = _musicVolume;
        activeSource.Play();
    }

    public void PlaySFX(SoundType soundType, float volume)
    {
        AudioClip clip = _soundList[(int)soundType];

        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume * _sfxVolume);
        }
        else
        {
            Debug.LogWarning("Audio clip not found for sound type: " + soundType);
        }
    }

    public void ChangeMusic(SoundType newSoundType, float volume)
    {
        AudioSource activeSource = _firstAudioSourceIsPlaying ? audioSource : audioSource2;

        activeSource.Stop();
        activeSource.clip = _soundList[(int)newSoundType];
        activeSource.volume = _musicVolume;
        activeSource.Play();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref _soundList, names.Length);

        for (int i = 0; i < _soundList.Length; i++)
        {
            if (_soundList[i] != null)
            {
                _soundList[i].name = names[i];
            }
        }
    }
#endif
}
