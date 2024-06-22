using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _floorMask;

    private IAttack _swipe;
    private IAttack _fireAttack;
    private IAttack _iceAttack;
    private IAttack _thunderAttack;
    private IAttack _groundAttack;

    [SerializeField] private GameObject _trail;
    [SerializeField] private GameObject _fireBullet;
    [SerializeField] private GameObject _iceBullet;
    [SerializeField] private GameObject _thunderBullet;
    [SerializeField] private GameObject _groundBullet;


    [Header("Animator")]
    Animator _animator;
    private Vector2 _lastMovement = Vector2.zero;

    private void Start()
    {
        _swipe = new Swipe(transform,_trail);
        _fireAttack = new FireAttack(transform, _trail, _fireBullet);
        _iceAttack = new IceAttack(transform, _trail, _iceBullet);
        _thunderAttack = new ThunderAttack(transform, _trail, _iceBullet);
        _groundAttack = new GroundAttack(transform, _trail, _iceBullet);

        _animator = GetComponent<Animator>();
        JoystickController.MoveEvent += UpdateAnimations;

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
                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    if (swipeDirection.x > 0)
                    {
                        _thunderAttack.SwipeDetection();
                    }
                    else
                    {
                        _groundAttack.SwipeDetection();
                    }
                }
                else
                {
                    if (swipeDirection.y > 0)
                    {
                        _fireAttack.SwipeDetection();
                    }
                    else
                    {
                        _iceAttack.SwipeDetection();
                    }
                }
            }
        }
    }

    void UpdateAnimations(Vector2 movement)
    {
        if (!GameManager.instance.IsPaused())
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
                _animator.SetFloat("HAx", _lastMovement.x);
                _animator.SetFloat("VAx", _lastMovement.y);
            }
        }
    }

    private void OnDestroy()
    {
        JoystickController.MoveEvent -= UpdateAnimations;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.IsPaused())
        {
            if (collision.gameObject.tag == "Coin")
            {
                // decirle al game manager che sumame 10 punteques
                GameManager.instance.GiveCurrency(10);
                Destroy(collision.gameObject);
            }
        }
    }
}