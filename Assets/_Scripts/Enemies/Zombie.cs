using UnityEngine;

public class Zombie : Enemy
{ 
    [Header("Components")]
    [SerializeField] Transform _player;
    [SerializeField] LayerMask _playerLayerMask;
    private Rigidbody2D _rb;
    private Animator _animator;

    [Header("Values")]
    [SerializeField] float _maxSpeed = 5f;
    [SerializeField] float _detectionRadius = 2f;
    [SerializeField] float _damage = 25f;
    [SerializeField] float _minDistanceToPlayer = 0.8f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (IsPlayerDetected()) SteeringBehaviour();
        else
        {
            StopMovement();
            UpdateAnimations(Vector2.zero);
        }
    }

    public override Vector3 GetPosition()
    {
        return transform.position;
    }

    protected override void SteeringBehaviour()
    {
        if (_player != null)
        {
            Vector2 playerDirection = _player.position - transform.position;

            float distance = playerDirection.magnitude;
            float lookAhead = distance / _maxSpeed;

            if (distance > _minDistanceToPlayer)
            {
                Vector2 playerPosition = (Vector2)_player.position + _player.GetComponent<Rigidbody2D>().velocity * lookAhead;
                Vector2 desiredVelocity = (playerPosition - (Vector2)transform.position).normalized * _maxSpeed;
                Vector2 steeringForce = desiredVelocity - GetComponent<Rigidbody2D>().velocity;

                GetComponent<Rigidbody2D>().AddForce(steeringForce);

                UpdateAnimations(desiredVelocity);
            }
            else
            {
                StopMovement();
                // Agregar animacion de ataque aca
            }
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

    void UpdateAnimations(Vector2 movement)
    {
        if (movement.magnitude > 0)
        {
            _animator.SetBool("isWalking", true);
            _animator.SetFloat("HAx", movement.x);
            _animator.SetFloat("VAx", movement.y);
        }
        else
        {
            _animator.SetBool("isWalking", false);
            _animator.SetFloat("HAx", movement.x);
            _animator.SetFloat("VAx", movement.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopMovement();
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(_damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
