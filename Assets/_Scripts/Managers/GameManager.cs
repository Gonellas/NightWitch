using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Park Timer")]
    [SerializeField] private float countdownEnd;
    [SerializeField] private float timer;
    [SerializeField] private bool isCounting = true;

    [Header("Game Manager Instance")]
    public static GameManager instance;

    [Header("Components References")]
    [SerializeField] Player player;
    [SerializeField] PlayerHealth _playerHealth;

    [Header("Save, Load, Delete Game Values")]
    public int _currency = 0;
    [SerializeField] int _energy = 10;
    [SerializeField] string _playerName = "Default";
    [SerializeField] TextMeshProUGUI[] _textShowingStats;
    [SerializeField] GameObject _deleteConfirmationPanel;
    [SerializeField] GameObject _canvasMainMenu;
    [SerializeField] GameObject _loseCanvas;
    [SerializeField] GameObject _winCanvas;
    [SerializeField] Button _storeShieldButton;

    [Header("PowerUps")]
    [SerializeField] GameObject _shieldButton;
    public bool _shieldBought = false;
    public int shieldLevel = 0;
    private readonly int[] _shieldCosts = { 50, 100, 150 };

    private List<PowerUpType> _boughtPowerUp = new List<PowerUpType>();

    [Header("Pause Game")]
    private bool isPaused = false;

    [Header("Energy Recovery")]
    [SerializeField] float _interval = 30f;
    [SerializeField] float _timer = 0f;

    [SerializeField] int _maxStamina = 10;
    int _currentStamina;
    [SerializeField] float _timer2ToRecharge = 10;

    bool recharging;

    [SerializeField] TextMeshProUGUI _staminaText;
    [SerializeField] TextMeshProUGUI _timerText;

    DateTime _nextStaminaTime;
    DateTime _lastStaminaTime;

    [SerializeField] string _titleNotif = "Full Stamina";
    [SerializeField] string _textNotif = "Stamina Full, Come Back to Play!";
    [SerializeField] IconSelecter _smallIcon = IconSelecter.icon_reminder;
    [SerializeField] IconSelecter _largeIcon = IconSelecter.icon_reminderbig;
    TimeSpan timer2;
    int id;

    private void Awake()
    {
        instance = this;

        _currentStamina = _maxStamina;
        _nextStaminaTime = DateTime.Now;
        _lastStaminaTime = DateTime.Now;

        LoadGame();
    }

    private void Start()
    {
        StartCoroutine(RechargeStamina());
        if (_currentStamina < _maxStamina)
        {
            timer2 = _nextStaminaTime - DateTime.Now;

            id = NotificationToSend();
        }
    }

    IEnumerator RechargeStamina()
    {
        UpdateStamina();
        UpdateTimer();
        recharging = true;

        while (_currentStamina < _maxStamina)
        {
            DateTime current = DateTime.Now;
            DateTime nextTime = _nextStaminaTime;

            bool addingStamina = false;

            while (current > nextTime)
            {
                if (_currentStamina >= _maxStamina) break;

                _currentStamina++;
                addingStamina = true;
                UpdateStamina();

                DateTime timeToAdd = nextTime;

                if (_lastStaminaTime > nextTime) timeToAdd = _lastStaminaTime;

                nextTime = AddDuration(timeToAdd, _timer2ToRecharge);
            }

            if (addingStamina)
            {
                _nextStaminaTime = nextTime;
                _lastStaminaTime = DateTime.Now;
            }

            UpdateTimer();
            UpdateStamina();
            SaveData();

            yield return new WaitForEndOfFrame();
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.CancelNotification(id);
        }
        else
        {
            Debug.LogWarning("NotificationManager.Instance is null!");
        }

        recharging = false;
    }

    DateTime AddDuration(DateTime timeToAdd, float timerToRecharge)
    {
        return timeToAdd.AddSeconds(timerToRecharge);
    }

    public void UseStamina(int staminaToUse)
    {
        if (_currentStamina - staminaToUse >= 0)
        {
            _currentStamina -= staminaToUse;

            if (!recharging)
            {
                _nextStaminaTime = AddDuration(DateTime.Now, _timer2ToRecharge);
                StartCoroutine(RechargeStamina());

                if (NotificationManager.Instance != null)
                {
                    NotificationManager.Instance.CancelNotification(id);

                    id = NotificationToSend();
                }
                else
                {
                    Debug.LogWarning("NotificationManager.Instance is null!");
                }
            }
        }
        else
        {
            Debug.Log("You don't have enough Stamina");
        }
    }

    int NotificationToSend()
    {
        if (NotificationManager.Instance != null)
        {
            return NotificationManager.Instance.DisplayNotification(_titleNotif,
                        _textNotif, _smallIcon, _largeIcon,
                        AddDuration(DateTime.Now, ((_maxStamina - _currentStamina + 1) * _timer2ToRecharge) + 1 + (float)timer2.TotalSeconds));
        }
        else
        {
            Debug.LogWarning("NotificationManager.Instance is null! Notification not sent.");
            return -1; 
        }
    }

    void UpdateStamina()
    {
        if (_staminaText != null)
        {
            _staminaText.text = $"{_currentStamina} / {_maxStamina}";
        }
        else
        {
            Debug.LogWarning("_staminaText is null!");
        }
    }

    void UpdateTimer()
    {
        if (_timerText != null)
        {
            if (_currentStamina >= _maxStamina)
            {
                _timerText.text = "Full Stamina!";
                return;
            }

            TimeSpan timer = _nextStaminaTime - DateTime.Now;

            _timerText.text = $"{timer.Hours.ToString("00")} : {timer.Minutes.ToString("00")} : {timer.Seconds.ToString("00")}";
        }
        else
        {
            Debug.LogWarning("_timerText is null!");
        }
    }

    private void LoadData()
    {
        _currentStamina = PlayerPrefs.GetInt(PlayerPrefsKey.currentStamina, _maxStamina);
        _nextStaminaTime = StringToDateTime(PlayerPrefs.GetString("DateTime_NextStaminaTime"));
        _lastStaminaTime = StringToDateTime(PlayerPrefs.GetString("DateTime_LastStaminaTime"));
    }

    DateTime StringToDateTime(string date)
    {
        if (string.IsNullOrEmpty(date))
            return DateTime.Now;
        else
            return DateTime.Parse(date);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.currentStamina, _currentStamina);
        PlayerPrefs.SetString("DateTime_NextStaminaTime", _nextStaminaTime.ToString());
        PlayerPrefs.SetString("DateTime_LastStaminaTime", _lastStaminaTime.ToString());
        PlayerPrefs.Save();
    }

    void Update()
    {
        // Park Timer
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            ParkTimer();
        }

        // Energy Recovery
        if (_energy < 10)
        {
            _timer += Time.deltaTime;
            if (_timer >= _interval)
            {
                _timer = 0f;
                GiveEnergy(1);
                SaveGame();
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            if (_textShowingStats.Length >= 4)
            {
                _textShowingStats[0].text = $"Currency: {_currency}";
                _textShowingStats[1].text = $"Energy: {_energy}";
                _textShowingStats[2].text = $"Player Name: {_playerName}";
                _textShowingStats[3].text = $"Time: {(int)timer}";

                if (shieldLevel > 0)
                {
                    _shieldButton.SetActive(_shieldBought);
                }
            }
        }
    }

    #region Lose/Win Conditions
    // Lose Condition
    public void Lose()
    {
        TogglePause();
        _loseCanvas.SetActive(true);
    }

    // Win Condition
    public void Win()
    {
        TogglePause();
        _winCanvas.SetActive(true);
    }
    #endregion

    #region PowerUps
    public bool PowerUpBought(PowerUpType powerUpType)
    {
        return _boughtPowerUp.Contains(powerUpType);
    }

    public void BuyPowerUp(PowerUpType powerUpType)
    {
        _boughtPowerUp.Add(powerUpType);
    }

    #region Shield
    public void BuyShield()
    {
        if (shieldLevel < _shieldCosts.Length && _currency >= _shieldCosts[shieldLevel])
        {
            TakeCurrency(_shieldCosts[shieldLevel]);
            shieldLevel++;
            _shieldBought = true;
            BuyPowerUp(PowerUpType.Shield);
            Debug.Log($"Escudo nivel {shieldLevel} comprado");
            SaveGame();
        }
    }
    #endregion
    #endregion

    public void RestartLevel()
    {
        TakeEnergy(1);
        SaveGame();
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
        isPaused = false;
    }

    #region Game Paused
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
    #endregion

    #region Save, Load, Delete Game
    private void SaveGame()
    {
        PlayerPrefs.SetInt("Data_Currency", _currency);
        PlayerPrefs.SetInt("Data_Energy", _energy);
        PlayerPrefs.SetString("Data_Name", _playerName);
        PlayerPrefs.SetInt("Data_ShieldBought", _shieldBought ? 1 : 0);
        PlayerPrefs.SetInt("Data_ShieldLevel", _shieldBought ? shieldLevel : 0);
        PlayerPrefs.Save();

        Debug.Log("Game Saved");
    }

    private void LoadGame()
    {
        _currency = PlayerPrefs.GetInt("Data_Currency", 0);
        _energy = PlayerPrefs.GetInt("Data_Energy", 10);
        _playerName = PlayerPrefs.GetString("Data_Name", "Default");
        _shieldBought = PlayerPrefs.GetInt("Data_ShieldBought", 0) == 1;
        shieldLevel = PlayerPrefs.GetInt("Data_ShieldLevel", 0);

        Debug.Log("Game Loaded");
    }

    public void DeleteGame()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        _deleteConfirmationPanel.SetActive(true);
    }

    public void ConfirmDeleteGame()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        PlayerPrefs.DeleteAll();
        Debug.Log("Game Deleted");
        LoadGame();
        _deleteConfirmationPanel.SetActive(false);
        _canvasMainMenu.SetActive(true);
    }

    public void CancelDeleteGame()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        _deleteConfirmationPanel.SetActive(false);
        _canvasMainMenu.SetActive(true);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }
    #endregion

    #region Get, Take Currency/Energy
    public void GiveCurrency(int add)
    {
        _currency += add;
    }

    public void TakeCurrency(int take)
    {
        _currency -= take;
    }

    public void GiveEnergy(int add)
    {
        _energy += add;
    }

    public void TakeEnergy(int take)
    {
        _energy -= take;
    }
    #endregion

    #region Buttons
    // Play Button
    public void PlayButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        TakeEnergy(1);
        AudioManager.Instance.ChangeMusic(SoundType.MainTheme_2, 100);
        SaveGame();
        SceneManager.LoadScene(5);
    }

    // Store Button
    public void StoreButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        SaveGame();
        SceneManager.LoadScene(1);
    }

    // Options Button
    public void OptionsButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        SaveGame();
        SceneManager.LoadScene(4);
    }

    // Tutorial Button
    public void TutorialButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        SaveGame();
        SceneManager.LoadScene(2);
    }

    public void ParkButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        AudioManager.Instance.ChangeMusic(SoundType.MainTheme_2, 100);
        SaveGame();
        SceneManager.LoadScene(3);
    }

    // Main Menu Button
    public void MainMenuButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        SaveGame();
        SceneManager.LoadScene(0);
        isPaused = false;
        Time.timeScale = 1;
    }

    // Application Quit
    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }
    #endregion

    // Park Timer
    private void ParkTimer()
    {
        if (isCounting)
        {
            timer -= Time.deltaTime;

            if (timer <= countdownEnd)
            {
                isCounting = false;
                GiveCurrency(100);
                SaveGame();
                Win();
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
