using UnityEngine;

public class Shield : PowerUp
{
    private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _shieldPrefab;
    private float[] _durations = { 3f, 5f, 8f };
    private Color[] _colors = {Color.blue, Color.green, Color.red};

    public Shield(GameObject shieldPrefab, PlayerHealth playerHealth) : base(0, 0, shieldPrefab)
    {
        _playerHealth = playerHealth;
        _shieldPrefab = shieldPrefab;
    }

    public void UpdateShieldProperties(int shieldLevel)
    {
        _durationPowerUp = _durations[shieldLevel - 1];
        Debug.Log(_durationPowerUp);
        _shieldPrefab.GetComponent<Renderer>().material.color = _colors[shieldLevel - 1];
    }

    public override void ActivePowerUp()
    {
        if (_playerHealth != null)
        {
            base.ActivePowerUp();
            _playerHealth.canTakeDamage = false;
        }
    }

    public override void DeactivatePowerUp()
    {
        if (_playerHealth != null)
        {
            base.DeactivatePowerUp();
            _playerHealth.canTakeDamage = true;
            Debug.Log("Shield: Player canTakeDamage set to true");
        }
    }
}