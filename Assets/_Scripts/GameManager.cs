using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Manager Instance
    public static GameManager instance;


    // References
    [SerializeField] Player player;


    //Save, Load, Delete Game:
    [SerializeField] int _currency = 0;
    [SerializeField] int _energy = 10;
    [SerializeField] string _playerName = "Default";
    [SerializeField] TextMeshProUGUI[] _textShowingStats;

    private void Awake()
    {
        instance = this;

        //Save, Load, Delete Game:
        LoadGame();
    }

    void Update()
    {
        //Save, Load, Delete Game:

        _textShowingStats[0].text = $"Currency: {_currency}";
        _textShowingStats[1].text = $"Energy: {_energy}";
        _textShowingStats[2].text = $"Player Name: {_playerName}";
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

        PlayerPrefs.DeleteAll();   // - Para borrar todas las keys. 

        Debug.Log("Deleting Game");
        LoadGame();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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


    //Get currency:

    public void GiveCurrency(int add)
    {
        _currency += add;
    }
}
