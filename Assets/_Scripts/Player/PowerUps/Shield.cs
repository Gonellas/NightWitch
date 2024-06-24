using System.Collections;
using UnityEngine;

public class Shield : IPowerUp
{
    private PlayerHealth _playerHealth;
    private GameObject _shieldObject;
    private Coroutine _shieldCoroutine;

    public Shield(PlayerHealth playerHealth, GameObject shieldObject)
    {
        _playerHealth = playerHealth;
        _shieldObject = shieldObject;
    }

    public void ApplyPowerUp()
    {
        Debug.Log("Shield: ApplyPowerUp called");

        if (_playerHealth != null)
        {
            _playerHealth.canTakeDamage = false;
            Debug.Log("Shield: Player canTakeDamage set to false");
        }

        if (_shieldObject != null)
        {
            if (_shieldCoroutine != null)
            {
                DeactivatePowerUp();
            }
        }
    }

    public void DeactivatePowerUp()
    {
        Debug.Log("Shield: DeactivatePowerUp called");

        if (_playerHealth != null)
        {
            _playerHealth.canTakeDamage = true;
            Debug.Log("Shield: Player canTakeDamage set to true");
        }

        if (_shieldObject != null)
        {
            _shieldObject.SetActive(false);
            Debug.Log("Shield: Shield deactivated");
        }
    }  
}