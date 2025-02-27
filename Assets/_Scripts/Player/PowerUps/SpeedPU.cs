using System.Collections;
using UnityEngine;

public class SpeedPU : PowerUp
{
    [SerializeField] private float _speedBoost = 5f;
    [SerializeField] private GameObject _speedBoostPrefab;
    [SerializeField] Player _player;
    [SerializeField] private float _originalSpeed = 5f;
    public float duration = 3f;
    private bool isActive = false;
    protected SpeedPU(float speedBoost, GameObject powerUpPrefab, Player player, float originalSpeed) : base(speedBoost, 0, powerUpPrefab)
    {
        _speedBoost = speedBoost;
        powerUpPrefab = _speedBoostPrefab;
        player = _player;
        _originalSpeed = originalSpeed;
    }

    public override void ActivePowerUp()
    {
        if (_player != null && !isActive)
        {
            isActive = true; 
            _player.speedPS.SetActive(true);
            _originalSpeed = _player._speed; 
            _player._speed += _speedBoost; 
            Debug.Log("Speed power up activated. New speed: " + _player._speed);

            Destroy(gameObject);

            _player.StartCoroutine(ActivateSpeed());
        }
    }

    private IEnumerator ActivateSpeed()
    {
        yield return new WaitForSeconds(duration); 

        if (_player != null && isActive)
        {
            _player._speed = _originalSpeed;
            _player.speedPS.SetActive(false);
            Debug.Log("Restored speed: " + _originalSpeed);
        }
    }
}