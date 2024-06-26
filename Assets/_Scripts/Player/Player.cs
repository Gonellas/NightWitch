using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _floorMask;

    [Header("Attacks")]
    private IAttack _swipe;
    private IAttack _fireAttack;
    private IAttack _iceAttack;
    private IAttack _thunderAttack;
    private IAttack _groundAttack;

    [Header("Bullets")]
    [SerializeField] private GameObject _trail;
    [SerializeField] private GameObject _fireBullet;
    [SerializeField] private GameObject _iceBullet;
    [SerializeField] private GameObject _thunderBullet;
    [SerializeField] private GameObject _groundBullet;

    private Vector2 _lastMovement = Vector2.zero;
    private EnemyDetector _enemyDetector;

    private void Start()
    {
        _swipe = new Swipe(transform, _trail);
        _fireAttack = new FireAttack(transform, _trail, _fireBullet);
        _iceAttack = new IceAttack(transform, _trail, _iceBullet);
        _thunderAttack = new ThunderAttack(transform, _trail, _thunderBullet);
        _groundAttack = new GroundAttack(transform, _trail, _groundBullet);

        _enemyDetector = GetComponent<EnemyDetector>();
    }

    void Update()
    {
        if (!GameManager.instance.IsPaused())
        {
            Vector2 swipeDirection = _swipe.SwipeDetection();
            Vector2 movement = _controller.GetMovementInput();

            transform.position += new Vector3(movement.x, movement.y, 0) * _speed * Time.deltaTime;

            if (movement.magnitude > 0)
            {
                _lastMovement = movement;
            }

            if (swipeDirection != Vector2.zero)
            {
                HandleAttack(swipeDirection);
            }
        }
    }

    private void HandleAttack(Vector2 swipeDirection)
    {
        Vector2 attackDirection = swipeDirection;

        // Use direction towards closest enemy if an enemy is detected
        if (_enemyDetector.ClosestEnemy != null)
        {
            Vector2 playerPosition = transform.position;
            Vector2 enemyDirection = _enemyDetector.GetDirectionToClosestEnemy(playerPosition);
            attackDirection = enemyDirection.normalized;
        }

        // Perform attack based on direction
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            if (attackDirection.x > 0)
            {
                _thunderAttack.SwipeDetection();
                EventManager.TriggerEvent(EventsType.Thunder_Attack, attackDirection);
            }
            else
            {
                _groundAttack.SwipeDetection();
                EventManager.TriggerEvent(EventsType.Ground_Attack, attackDirection);
            }
        }
        else
        {
            if (attackDirection.y > 0)
            {
                _fireAttack.SwipeDetection();
                EventManager.TriggerEvent(EventsType.Fire_Attack, attackDirection);
            }
            else
            {
                _iceAttack.SwipeDetection();
                EventManager.TriggerEvent(EventsType.Ice_Attack, attackDirection);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.IsPaused())
        {
            if (collision.gameObject.tag == "Coin")
            {
                GameManager.instance.GiveCurrency(10);
                Destroy(collision.gameObject);
            }
        }
    }
}

