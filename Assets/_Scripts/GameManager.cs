using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager Instance")]
    public static GameManager instance;

    [Header("Components References")]
    [SerializeField] Player player;
    [SerializeField] PlayerHealth _playerHealth;

    [Header("Save, Load, Delete Game Values")]
    [SerializeField] int _currency = 0;
    [SerializeField] int _energy = 10;
    [SerializeField] string _playerName = "Default";
    [SerializeField] TextMeshProUGUI[] _textShowingStats;
    [SerializeField] GameObject _deleteConfirmationPanel;
    [SerializeField] GameObject _canvasMainMenu;
    [SerializeField] GameObject _loseCanvas;

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
    }

    void Update()
    {
        //Energy Recovery
        if(_energy < 10)
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

    //Lose Condition
    public void Lose()
    {
        TogglePause();
        _loseCanvas.SetActive(true);
    }
    private void UpdateUI()
    {
        _textShowingStats[0].text = $"Currency: {_currency}";
        _textShowingStats[1].text = $"Energy: {_energy}";
        _textShowingStats[2].text = $"Player Name: {_playerName}";
    }

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

        PlayerPrefs.Save();

        Debug.Log("Saving Game");
    }

    //Save, Load, Delete Game:
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
        //PlayerPrefs.DeleteKey("Data_Currency");
        //PlayerPrefs.DeleteKey("Data_Life");
        //PlayerPrefs.DeleteKey("Data_Name");

        // Agregar confirmacion
        _canvasMainMenu.SetActive(false);
        _deleteConfirmationPanel.SetActive(true);

        /*PlayerPrefs.DeleteAll()*/;   // - Para borrar todas las keys. 

        //Debug.Log("Deleting Game");
        //LoadGame();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    //private void OnApplicationFocus(bool focus) -- Cuando minimizas que se guarde
    //{
    //    if (!focus) SaveGame();
    //}

    private void OnApplicationQuit()
    {
        SaveGame();
    }


    //Get, Take _currency:

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
    public void OptionsButton()
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
}
