using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public enum Language
{
    Spanish,
    English
}

public class Localization : MonoBehaviour
{
    [SerializeField] private string _webURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQxPQNur1nwAyv-98yjzRny_Iv81K6xOYHgC82NxF4eaKBN-uJHMF0iXgQ5qJzImo6OSNcwj0sA3d5U/pub?output=csv";

    [SerializeField] private Language _currentLang;

    private Dictionary<Language, Dictionary<string, string>> _languageCodex;
    
    public event Action OnUpdate = delegate { };
    
    private void Awake()
    {
        StartCoroutine(DownloadCSV(_webURL));
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _currentLang = (_currentLang == Language.English) ? Language.Spanish : Language.English;

            OnUpdate();

            /*
            if (_currentLang == Language.English)
            {
                _currentLang = Language.Spanish;
            }
            else
            {
                _currentLang = Language.English;
            }
            */
        }
    }
    
    IEnumerator DownloadCSV(string url)
    {
        var www = new UnityWebRequest(url);

        www.downloadHandler = new DownloadHandlerBuffer();

        //www.Abort();

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var result = www.downloadHandler.text;

            _languageCodex = LanguageSplit.LoadCSV(result, "web");

            SaveText(fileName: "Codex", content: result);
        }
        else
        {
            var result = LoadText("Codex");
            _languageCodex = LanguageSplit.LoadCSV(result, "local");
        }
        
        OnUpdate();
    }

    public string GetTranslate(string ID)
    {
        var idsDictionary = _languageCodex[_currentLang];

        idsDictionary.TryGetValue(ID, out var result);

        return result;
    }

    public void ChangeLanguage(Language newLang)
    {
        _currentLang = newLang;

        OnUpdate();
    }

    void SaveText(string fileName, string content)
    {
        string path = Application.persistentDataPath + "/" + fileName;

        try
        {
            File.WriteAllText(path, content);
            Debug.Log($"File saved successfully at: {path}");
        }
        catch(Exception ex)
        {
            Debug.LogError($"Failed to save file: {ex.ToString()}");
        }
    }

    string LoadText(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;

        try
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                Debug.Log($"File loaded successfully from: {path}");
                return content;
            }
            else
            {
                Debug.LogError($"File not found at: {path}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load file: {e.ToString()}");
            return null;
        }
    }

}
