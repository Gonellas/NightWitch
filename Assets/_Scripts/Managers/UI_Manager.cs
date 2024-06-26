using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Player player;
    private PowerUp _powerUp;
    private Shield _shield;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private Button _shieldButton;
    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private float _activeShield = 3f;

    [SerializeField] private bool _cooldownActive = false;

    [Header("Paused Game")]
    [SerializeField] GameObject _pauseButtonCanvas;
    [SerializeField] GameObject _pauseMenuCanvas;

    public static UI_Manager instance;

    public static UI_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UI_Manager>();

                if (instance == null)
                {
                    instance = new GameObject("UI_MANAGER", typeof(UI_Manager)).GetComponent<UI_Manager>();
                }
            }

            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _powerUp = FindObjectOfType<PowerUp>();
        _shield = new Shield(_shieldPrefab, FindObjectOfType<PlayerHealth>());

        UpdatePowerUpButtons();
    }
    #region Active PowerUp
    public void ActivateShield()
    {
        if (!GameManager.instance.IsPaused() && !_cooldownActive)
        {
            _shield.UpdateShieldProperties(GameManager.instance.shieldLevel);
            StartCoroutine(ActivatePowerUp(_shield, _shield._durationPowerUp));
        }
    }
    #endregion

    #region Set PowerUp

    private void UpdatePowerUpButtons()
    {
        if (GameManager.instance.PowerUpBought(PowerUpType.Shield)) 
        { 
            _shieldButton.gameObject.SetActive(true);
        }

        if (GameManager.instance.PowerUpBought(PowerUpType.Speed))
        {

        }
    }

    private IEnumerator ActivatePowerUp(IPowerUp powerUp, float duration)
    {
        powerUp.ActivePowerUp();
        yield return new WaitForSeconds(duration);
        powerUp.DeactivatePowerUp();

        _cooldownActive = true;
        UpdatePowerUpButtons();
        yield return new WaitForSeconds(_cooldown);
        _cooldownActive = false;
        UpdatePowerUpButtons();
    }
    #endregion

    #region Buttons
    public void PauseMenu()
    {
        GameManager.instance.TogglePause();
        _pauseButtonCanvas.SetActive(!GameManager.instance.IsPaused());
        _pauseMenuCanvas.SetActive(GameManager.instance.IsPaused());
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX(SoundType.Click, 1);
        GameManager.instance.TogglePause();
        _pauseButtonCanvas.SetActive(true);
        _pauseMenuCanvas.SetActive(false);
    }

    public void GoToMenuGame()
    {
        AudioManager.Instance.ChangeMusic(SoundType.MainTheme_1, 100);
        GameManager.instance.MainMenuButton();
    }

   
    #endregion
}
