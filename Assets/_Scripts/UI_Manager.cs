using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Player player;
    private IPowerUp _shield;
    private IPowerUp _powerUp;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] GameObject _shieldButton;


    [Header("Paused Game")]
    [SerializeField] GameObject _pauseButtonCanvas;
    [SerializeField] GameObject _pauseMenuCanvas;

    private void Start()
    {
        _shield = new Shield(FindObjectOfType<PlayerHealth>(), _shieldPrefab);

        UpdateShieldButton();

    }

    public void Shield()
    {
        if (!GameManager.instance.IsPaused())
        {
            SetPowerUp(_shield);
        }
    }

    public void BuyShield()
    {
        GameManager.instance.BuyShield();
        UpdateShieldButton(); 
    }

    private void UpdateShieldButton()
    {
        if (GameManager.instance._shieldBought)
        {
            _shieldButton.SetActive(true);
        }
        else _shieldButton.SetActive(false);
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

    public void SetPowerUp(IPowerUp powerUp)
    {
        if (_powerUp != null)
        {
            _powerUp.DeactivatePowerUp();
            StopCoroutine(ActivateShieldForTime());
        }

        // Asigna el nuevo PowerUp actual y lo activa si es diferente de null
        _powerUp = powerUp;
        if (_powerUp != null)
        {
            _powerUp.ApplyPowerUp();
            StartCoroutine(ActivateShieldForTime());
        }
    }

    private IEnumerator ActivateShieldForTime()
    {
        if (_shieldPrefab != null)
        {
            _shieldPrefab.SetActive(true);
            Debug.Log("Shield: Shield activated");
        }

        yield return new WaitForSeconds(5f);

        _shield.DeactivatePowerUp();
    }
}