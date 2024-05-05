using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveWithPlayerPrefs : MonoBehaviour
{
    [SerializeField] int _currency = 10;
    [SerializeField] float _life = 100;
    [SerializeField] string _playerName = "Default";
    [SerializeField] TextMeshProUGUI[] _textShowingStats;

    void Awake()
    {
        LoadGame();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S)) SaveGame();
        //else if (Input.GetKeyDown(KeyCode.L)) LoadGame();
        //else if (Input.GetKeyDown(KeyCode.D)) DeleteGame();

        _textShowingStats[0].text = $"Currency: {_currency}";
        _textShowingStats[1].text = $"Life: {_life}";
        _textShowingStats[2].text = $"Player Name: {_playerName}";
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("Data_Currency", _currency);

        PlayerPrefs.SetFloat("Data_Life", _life);

        PlayerPrefs.SetString("Data_Name", _playerName);

        PlayerPrefs.Save();

        Debug.Log("Saving Game");
    }

    private void LoadGame()
    {
        // if(PlayerPrefs.HasKey("Data_Currency")) _currency = PlayerPrefs.GetInt("Data_Currency");
        _currency = PlayerPrefs.GetInt("Data_Currency", 10);



       //  if (PlayerPrefs.HasKey("Data_Life")) _life = PlayerPrefs.GetFloat("Data_Life");
        _life = PlayerPrefs.GetFloat("Data_Life", 100);



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
}
