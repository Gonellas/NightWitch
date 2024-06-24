using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("PowerUps")]
    [SerializeField] GameObject _shieldButton;
    public bool _shieldBought = false;

    [Header("Pause Game")]
    private bool isPaused = false;

    [Header("Energy Recovery")]
    [SerializeField] float _interval = 30f;
    [SerializeField] float _timer = 0f;

    private void Awake()
    {
        instance = this;

        //Save, Load, Delete Game:
        LoadGame();

        CheckShieldBought();
    }

    void Update()
    {
        //Park Timer
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            ParkTimer();
        }

        //Energy Recovery
        if (_energy < 10)
        {
            _timer += Time.deltaTime;
            if(_timer >= _interval)
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
        _textShowingStats[0].text = $"Currency: {_currency}";
        _textShowingStats[1].text = $"Energy: {_energy}";
        _textShowingStats[2].text = $"Player Name: {_playerName}";
        _textShowingStats[3].text = $"Time: {(int)timer}";

        _shieldButton.SetActive(_shieldBought);
    }

    #region Lose/Win Conditions
    //Lose Condition
    public void Lose()
    {
        TogglePause();
        _loseCanvas.SetActive(true);
    }

    //Win Condition
    public void Win()
    {
        TogglePause();
        _winCanvas.SetActive(true);
    }
    #endregion

    public void BuyShield()
    {
        if(_currency >= 50)
        {
            TakeCurrency(50);
            _shieldBought = true;
            Debug.Log("Escudo comprado");
            SaveGame();
        }
    }

    private void CheckShieldBought()
    {
        _shieldBought = PlayerPrefs.GetInt("Data_ShieldBought", 0) == 1;
    }

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

    #region Save, Load, Delete Game:
    private void SaveGame()
    {
        PlayerPrefs.SetInt("Data_Currency", _currency);

        PlayerPrefs.SetInt("Data_Energy", _energy);

        PlayerPrefs.SetString("Data_Name", _playerName);

        PlayerPrefs.SetInt("Data_ShieldBought", _shieldBought ? 1 : 0);

        PlayerPrefs.Save();

        Debug.Log("Saving Game");
    }

    private void LoadGame()
    {
        // if(PlayerPrefs.HasKey("Data_Currency")) _currency = PlayerPrefs.GetInt("Data_Currency");
        _currency = PlayerPrefs.GetInt("Data_Currency", 0);



        //  if (PlayerPrefs.HasKey("Data_Life")) _life = PlayerPrefs.GetFloat("Data_Life");
        _energy = PlayerPrefs.GetInt("Data_Energy", 10);



        //if (PlayerPrefs.HasKey("Data_Name")) _playerName = PlayerPrefs.GetString("Data_Name");
        _playerName = PlayerPrefs.GetString("Data_Name", "Default");

        Debug.Log("Loading Game");
    }

    public void DeleteGame()
    {
        // Agregar confirmacion
        _canvasMainMenu.SetActive(false);
        _deleteConfirmationPanel.SetActive(true);
    }

    public void ConfirmDeleteGame()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Deleting Game");
        LoadGame(); 

        _deleteConfirmationPanel.SetActive(false);
        _canvasMainMenu.SetActive(true);

    }

    public void CancelDeleteGame()
    {
        _deleteConfirmationPanel.SetActive(false);
        _canvasMainMenu.SetActive(true);
    }

    private void OnApplicationPause(bool pause) // -- Cuando pausas que se guarde
    {
        if (pause) SaveGame();
    }
    #endregion



    #region Get, Take Currency/Energy:

    public void GiveCurrency(int add)
    {
        _currency += add;
    }

    public void TakeCurrency(int take)
    {
        _currency -= take;
    }

    public void GiveEnergy (int add)
    {
        _energy += add;
    }

    public void TakeEnergy(int take)
    {
        _energy -= take;
    }
    #endregion

    #region Buttons
    //Play Button
    public void PlayButton()
    {
        TakeEnergy(1);
        SaveGame();
        SceneManager.LoadScene(3);
    }

    //Store Button
    public void StoreButton()
    {
        SaveGame();
        SceneManager.LoadScene(1);
    }

    //Options Button
    public void TutorialButton()
    {
        SaveGame();
        SceneManager.LoadScene(2);
    }

    //Main Menu Button
    public void MainMenuButton()
    {
        SaveGame();
        SceneManager.LoadScene(0);
        isPaused = false;
        Time.timeScale = 1;
    }

    //Application Quit
    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }
    #endregion

    //Park Timer
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
