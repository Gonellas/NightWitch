using System;
using UnityEngine;

public enum SoundType
{
    MainTheme_1,
    MainTheme_2,
    Fire_Attack,
    Ice_Attack,
    GroundAttack,
    ThunderAttack,
    Shield,
    Player_Movement,
    Player_Damaged,
    Player_Die,
    Zombie_Movement,
    Zombie_Damaged,
    Zombie_Die,
    Zombie_Attack,
    Fairy_Movement,
    Fairy_Damaged,
    Fairy_Die,
    Fairy_Bomb
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

    [SerializeField] private AudioClip[] _soundList;

    [SerializeField] private bool _firstAudioSourceIsPlaying;


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

        private set
        {
            instance = value;
        }
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(SoundType.MainTheme_2, 100);
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
    }

    public float GetSFXVolume()
    {
        return _sfxVolume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = _sfxVolume;
    }

    public void PlayMusic(SoundType soundType, float volume) 
    {
        AudioSource activeSource = _firstAudioSourceIsPlaying ? audioSource : audioSource2;

        activeSource.PlayOneShot(_soundList[(int)soundType], _musicVolume);
    }

    public void PlaySFX(SoundType soundType, float volume)
    {
        sfxSource.PlayOneShot(_soundList[(int)soundType], _sfxVolume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref _soundList, names.Length);

        for (int i = 0; i < _soundList.Length; i++)
        {
            _soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] _sounds;
}
