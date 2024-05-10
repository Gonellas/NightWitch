using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _floorMask;
    private PlayerHealth playerHealth;
    private bool isDamaged = false;

    [SerializeField] private Material enemyMaterial;

    private Enemy closestEnemy = null;

    public Enemy ClosestEnemy { get; private set; }

    private Vector2 initialTouch;
    private Vector2 finalTouch;

    [SerializeField] private GameObject trail;
    private GameObject currentTrail;

    [Header("Animator")]
    Animator _animator;
    private Vector2 _lastMovement = Vector2.zero;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        JoystickController.MoveEvent += UpdateAnimations;
    }

    void Update()
    {
        if (!GameManager.instance.IsPaused()) 
        {
            Vector2 movement = _controller.GetMovementInput();

            transform.position += new Vector3(_controller.GetMovementInput().x, _controller.GetMovementInput().y, 0) * _speed * Time.deltaTime;

            if (movement.magnitude > 0)
            {
                _lastMovement = movement;
            }

            FindClosestEnemy();
            SwipeDetection();
        }
    }



    private void SwipeDetection()
    {
        if (!GameManager.instance.IsPaused())
        {
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(Input.touchCount - 1);

                if (touch.position.x >= Screen.width * 0.5f && touch.position.y <= Screen.height * 0.5f)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        initialTouch = touch.position;

                        if (currentTrail != null)
                        {
                            Destroy(currentTrail);

                        }
                        currentTrail = Instantiate(trail);

                        currentTrail.transform.position = touch.position;

                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        currentTrail.transform.position = touch.position;
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        finalTouch = touch.position;

                        Vector2 swipeDirection = (finalTouch - initialTouch);

                        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                        {

                            if (swipeDirection.x > 0)
                            {
                                Debug.Log("Swiped right");
                            }
                            else
                            {
                                Debug.Log("Swiped left");
                            }
                        }
                        else
                        {
                            // Vertical swipe
                            if (swipeDirection.y > 0)
                            {
                                Debug.Log("Swiped up");
                            }
                            else
                            {
                                Debug.Log("Swiped down");
                            }
                        }

                        Destroy(currentTrail);
                    }
                }
            }
        }
    }

    public void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                if (closestEnemy)
                {
                    var renderer2 = closestEnemy.GetComponent<MeshRenderer>();
                    renderer2.material = enemyMaterial;
                }

                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;

                var renderer = currentEnemy.GetComponent<MeshRenderer>();
                renderer.material = default;
            }
        }

        ClosestEnemy = closestEnemy;
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