using UnityEngine;
using System.Collections;

public enum PowerUpType
{
    Shield,
    Speed
}

public class PowerUp : MonoBehaviour, IPowerUp
{
    public float _durationPowerUp; 
    protected float _cooldownPowerUp;
    protected GameObject _powerUpPrefab;
    private bool _isActive = false;
    

    protected PowerUp(float duration, float cooldown, GameObject powerUpPrefab)
    {
        _durationPowerUp = duration;
        _cooldownPowerUp = cooldown;
        _powerUpPrefab = powerUpPrefab;
    }
    public virtual void ActivePowerUp()
    {
        _isActive = true;
        Debug.Log($"PowerUp applied: {_powerUpPrefab.name}");
        _powerUpPrefab.SetActive(true);
    }

    public virtual void DeactivatePowerUp()
    {
        _isActive = false;
        Debug.Log($"PowerUp deactivated: {_powerUpPrefab.name}");
        _powerUpPrefab.SetActive(false);
    }
}