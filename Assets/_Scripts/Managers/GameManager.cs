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
    public int _currency = 200;
    [SerializeField] int _energy = 10;
    [SerializeField] string _playerName = "Default";
    [SerializeField] TextMeshProUGUI[] _textShowingStats;
    [SerializeField] GameObject _deleteConfirmationPanel;
    [SerializeField] GameObject _canvasMainMenu;
    [SerializeField] GameObject _loseCanvas;
    [SerializeField] GameObject _winCanvas;
    [SerializeField] Button _storeShieldButton;

    [Header("STORE")]
    [SerializeField] private TextMeshProUGUI _shieldLevelToBuy;
    [SerializeField] private TextMeshProUGUI _shieldCostText;
    [SerializeField] private GameObject _shieldCostTextObject;
    [SerializeField] private GameObject _noCurrencyPanel;
    [SerializeField] private GameObject _confirmShieldPanel;
    [SerializeField] private Button _buyButton;
    [SerializeField] private GameObject _buyButtonObject;
    [SerializeField] private GameObject _confirmEnergyPanel;



    [Header("PowerUps")]
    [SerializeField] GameObject _shieldButton;
    public bool _shieldBought;
    public int shieldLevel;
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
        _nextStaminaTime = AddDuration(DateTime.Now, _interval);

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
                //_nextStaminaTime = nextTime;
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

        recharging = false;
    }

    DateTime AddDuration(DateTime timeToAdd, float timerToRecharge)
    {
        return timeToAdd.AddSeconds(timerToRecharge);
    }

    public void UseStamina(int staminaToUse)
    {
        Debug.Log(DateTime.Now);
        _nextStaminaTime = AddDuration(DateTime.Now, _interval);
        if (_energy - staminaToUse >= 0)
        {
            _energy -= staminaToUse;

            //if (!recharging)
            {
                StartCoroutine(RechargeStamina());

                if (NotificationManager.Instance != null)
                {
                    NotificationManager.Instance.CancelNotification(id);

                    id = NotificationToSend();
                }
            }
        }
    }

    int NotificationToSend()
    {
        if (NotificationManager.Instance != null)
        {
            return NotificationManager.Instance.DisplayNotification(_titleNotif,
                        _textNotif, _smallIcon, _largeIcon,
                        AddDuration(DateTime.Now, ((_maxStamina - _energy + 1) * _interval)));
        }
        else
        {
            return -1;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            NotificationToSend();
        }
    }



    void UpdateStamina()
    {
        if (_staminaText != null)
        {
            _staminaText.text = $"{_currentStamina} / {_maxStamina}";
        }
    }

    void UpdateTimer()
    {
        if (_timerText != null)
        {
            if (_energy >= _maxStamina)
            {
                _timerText.text = "Full Stamina!";
                return;
            }

            TimeSpan timer = _nextStaminaTime - DateTime.Now;

            _timerText.text = $"{timer.Hours.ToString("00")} : {timer.Minutes.ToString("00")} : {timer.Seconds.ToString("00")}";
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
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            ParkTimer();
        }

        if (_energy < 10)
        {
            _timer += Time.deltaTime;
            if (_timer >= _interval)
            {
                _nextStaminaTime = AddDuration(DateTime.Now, _interval);
                _timer = 0f;
                GiveEnergy(1);
                SaveGame();
            }
        }
        UpdateTimer();
        UpdateUI();
    }

    private void UpdateUI()
    {

        if (_textShowingStats.Length >= 4)
        {
            _textShowingStats[0].text = $"{_currency}";
            _textShowingStats[1].text = $"{_energy}";
            if (_textShowingStats[2])
                _textShowingStats[2].text = $"Player Name: {_playerName}";
            if (_textShowingStats[3])
                _textShowingStats[3].text = $"Time: {(int)timer}";

            if (shieldLevel > 0)
            {
                _shieldButton.SetActive(_shieldBought);
            }
        }

    }

    public void Lose()
    {
        TogglePause();
        _loseCanvas.SetActive(true);
    }

    public void Win()
    {
        TogglePause();
        _winCanvas.SetActive(true);

        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            PlayerPrefs.SetInt("Level1Completed", 1);
        }
    }

    #region STORE
    public bool PowerUpBought(PowerUpType powerUpType)
    {
        return _boughtPowerUp.Contains(powerUpType);
    }

    public void BuyPowerUp(PowerUpType powerUpType)
    {
        _boughtPowerUp.Add(powerUpType);
    }

    public void BuyShield()
    {
        if (shieldLevel < _shieldCosts.Length && _currency >= _shieldCosts[shieldLevel])
        {
            TakeCurrency(_shieldCosts[shieldLevel]);
            shieldLevel++;
            _shieldBought = true;
            BuyPowerUp(PowerUpType.Shield);
            SaveGame();
        }
    }

    public void UpdateShieldPanelText()
    {
        int level = shieldLevel;

        if (level >= _shieldCosts.Length)
        {
            _shieldLevelToBuy.text = "Shield Max Level";
            _shieldCostTextObject.SetActive(false);
            _buyButtonObject.SetActive(false);
            return;
        }

        int cost = _shieldCosts[level];
        _shieldLevelToBuy.text = $"Shield Lvl{level + 1}";
        _shieldCostText.text = $"{cost}";

        if (_currency < cost)
        {
            _noCurrencyPanel.SetActive(true);
        }
        else
        {
            _noCurrencyPanel.SetActive(false);
        }
    }

    public void OpenShieldConfirmPanel()
    {
        UpdateShieldPanelText();
        _confirmShieldPanel.SetActive(true);
    }

    public void TryBuyShield()
    {
        int level = shieldLevel;

        if (level >= _shieldCosts.Length) return;

        int cost = _shieldCosts[level];

        if (_currency >= cost)
        {
            BuyShield();
            _confirmShieldPanel.SetActive(false);
        }
        else
        {
            _confirmShieldPanel.SetActive(false);
            _noCurrencyPanel.SetActive(true);
        }
    }

    public void TryBuyEnergy()
    {
        if (_currency >= 100)
        {
            TakeCurrency(100);
            GiveEnergy(1);
        }
        else
        {
            _confirmEnergyPanel.SetActive(false);
            _noCurrencyPanel.SetActive(true);
        }
    }

    #endregion

    public void RestartLevel()
    {
        TakeEnergy(1);
        SaveGame();
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("Data_Currency", _currency);
        PlayerPrefs.SetInt("Data_Energy", _energy);
        PlayerPrefs.SetString("Data_Name", _playerName);
        PlayerPrefs.SetInt("Data_ShieldBought", _shieldBought ? 1 : 0);
        PlayerPrefs.SetInt("Data_ShieldLevel", _shieldBought ? shieldLevel : 0);
        PlayerPrefs.Save();
    }

    private void LoadGame()
    {
        _currency = PlayerPrefs.GetInt("Data_Currency", 0);
        _energy = PlayerPrefs.GetInt("Data_Energy", 10);
        _playerName = PlayerPrefs.GetString("Data_Name", "Default");

        if (SceneManager.GetActiveScene().buildIndex == 2)
            return;

        _shieldBought = PlayerPrefs.GetInt("Data_ShieldBought", 0) == 1;
        shieldLevel = PlayerPrefs.GetInt("Data_ShieldLevel", 0);
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

        if (pause)
        {
            NotificationToSend(); SaveGame();
        }
    }

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

    public bool TakeEnergy(int take)
    {
        if (_energy <= 0)
        {
            return false;
        }
        UseStamina(take);


        return true;
    }


    public void PlayButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        if (!TakeEnergy(1))
        {
            return;
        }

        AudioManager.Instance.ChangeMusic(SoundType.MainTheme_2, 100);
        SaveGame();

        int hasPlayed = PlayerPrefs.GetInt("HasPlayed", 0);
        int level1Completed = PlayerPrefs.GetInt("Level1Completed", 0);

        if (hasPlayed == 0)
        {
            PlayerPrefs.SetInt("HasPlayed", 1);
            /*SceneManager.LoadScene(2);*/ // Tutorial
            SceneTransitionManager.Instance.LoadSceneWithLoadingScreen("Tutorial");
        }
        else if (level1Completed == 0)
        {
            /*SceneManager.LoadScene(5);*/ // Nivel 1
            SceneTransitionManager.Instance.LoadSceneWithLoadingScreen("Graveyard");
        }
        else
        {
            /*SceneManager.LoadScene(3);*/ // Nivel 2
            SceneTransitionManager.Instance.LoadSceneWithLoadingScreen("Park");
        }
    }

    public void StoreButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        SaveGame();
        SceneManager.LoadScene(1);
    }

    public void OptionsButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        SaveGame();
        SceneManager.LoadScene(4);
    }

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

    public void MainMenuButton()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        SaveGame();
        SceneManager.LoadScene(0);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

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
        NotificationToSend();
        SaveGame();
    }
}
