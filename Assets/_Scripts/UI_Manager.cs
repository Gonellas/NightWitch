using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Player player;

    [Header("Paused Game")]
    [SerializeField] GameObject _pauseButtonCanvas;
    [SerializeField] GameObject _pauseMenuCanvas;

    public void FireBall()
    {
        if (!GameManager.instance.IsPaused())
        {
            var bullet = BulletFactory.Instance.GetObjectFromPool();
            bullet.transform.position = player.transform.position;
        }
    }

    public void PauseMenu()
    {
        GameManager.instance.TogglePause();
        _pauseButtonCanvas.SetActive(!GameManager.instance.IsPaused());
        _pauseMenuCanvas.SetActive(GameManager.instance.IsPaused());
    }

    public void ResumeGame()
    {
        GameManager.instance.TogglePause();
        _pauseButtonCanvas.SetActive(true);
        _pauseMenuCanvas.SetActive(false);
    }

    public void GoToMenuGame()
    {
        GameManager.instance.MainMenuButton();
    }
}

