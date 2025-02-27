using UnityEngine;

public class LifePU : PowerUp
{
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private GameObject _lifePUPrefab;
    protected LifePU(GameObject lifePUPrefab, PlayerHealth playerHealth) : base(0, 0, lifePUPrefab)
    {
        _playerHealth = playerHealth;
        _lifePUPrefab = lifePUPrefab;
    }

    private void LifeIncrease()
    {
        if(_playerHealth != null) 
        {
            if(_playerHealth.currentHealth < _playerHealth.maxHealth)
            {
                _playerHealth.currentHealth += 25;
                _playerHealth.UpdateHealthBar();
                Debug.Log("Recovered Life: " + _playerHealth.currentHealth);
            }
        }
    }

    public override void ActivePowerUp()
    {
        LifeIncrease();
        DeactivatePowerUp();
    }

    public override void DeactivatePowerUp()
    {
        GameObject.Destroy(_lifePUPrefab);
    }
}
