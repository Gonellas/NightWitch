using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;

    [SerializeField] private GameObject trigger1movement;
    [SerializeField] private GameObject trigger2spawnDummy;
    [SerializeField] private GameObject trigger3spawnZombieShield;
    [SerializeField] private GameObject trigger4spawnPotion;

    [SerializeField] private string text0;
    [SerializeField] private string text1;
    [SerializeField] private string text2;
    [SerializeField] private string text3;
    [SerializeField] private string text4;

    [SerializeField] private GameObject enemyDummy;
    [SerializeField] private GameObject enemyZombie;

    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject factory;

    private void Start()
    {
        tutorialText.text = text0;

        trigger2spawnDummy.SetActive(false);
        trigger3spawnZombieShield.SetActive(false);
        trigger4spawnPotion.SetActive(false);
    }

    public void OnTrigger1Hit()
    {
        tutorialText.text = text1;
        trigger2spawnDummy.SetActive(true);
    }

    public void OnTrigger2Hit()
    {
        tutorialText.text = text2;
        trigger3spawnZombieShield.SetActive(true);
    }

    public void OnTrigger3Hit()
    {
        tutorialText.text = text3;
        trigger4spawnPotion.SetActive(true);
    }

    public void OnTrigger4Hit()
    {
        tutorialText.text = text4;
        spawner.SetActive(true);
        factory.SetActive(true);
    }

    public void OnTrigger5Hit()
    {
        SceneManager.LoadScene(0); // Cargar la escena 0 (menú principal)
    }
}
