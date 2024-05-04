using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform _player;
    [SerializeField] LayerMask _playerLayerMask;
    private Rigidbody2D _rb;

    [Header("Values")]
    [SerializeField] float _maxSpeed = 5f;
    [SerializeField] float _detectionRadius = 2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (IsPlayerDetected()) PursueBehaviour();
        else StopMovement();
    }

    void PursueBehaviour()
    {
        if(_player != null)
        {
            Vector2 playerDirection = _player.position - transform.position;
            
            float distance = playerDirection.magnitude;
            float lookAhead = distance / _maxSpeed;

            Vector2 playerPosition = (Vector2)_player.position + _player.GetComponent<Rigidbody2D>().velocity * lookAhead;
            Vector2 desiredVelocity = (playerPosition - (Vector2)transform.position).normalized * _maxSpeed;
            Vector2 steeringForce = desiredVelocity - GetComponent<Rigidbody2D>().velocity;

            GetComponent<Rigidbody2D>().AddForce(steeringForce);
        }
    }

    void StopMovement()
    {
        _rb.velocity = Vector2.zero;
    }

    bool IsPlayerDetected()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _playerLayerMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _player = collider.transform;
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
