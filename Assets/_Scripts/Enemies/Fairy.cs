using System.Collections;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    [Header("Fairy Components and Values")]
    [SerializeField] Transform _player;
    [SerializeField] LayerMask _colliders;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _detectionRadius = 5f;
    [SerializeField] bool _isFleeing = false; 
    Animator _animator;
    
    [Header("Bomb Components and Values")]
    [SerializeField] GameObject _bomb; 
    [SerializeField] bool _canSpawnBomb = true;
    [SerializeField] float _bombCooldown = 10f;
    private float _lastBombSpawnTime; 


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (_player != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, _player.position);

            if (distanceToTarget <= _detectionRadius)
            {
                FleeDirection();

                if (_canSpawnBomb)
                {
                    _canSpawnBomb = false; 
                    StartCoroutine(ActiveBomb());

                }
            }
        }     
    }

    private IEnumerator ActiveBomb()
    {
        SpawnBomb();

        yield return new WaitForSeconds(1.5f);
    }

    private void SpawnBomb()
    {
        GameObject bomb = Instantiate(_bomb, transform.position, Quaternion.identity);
        bomb.transform.localScale = Vector3.one; 

        StartCoroutine(ExpandBomb(bomb));

        _lastBombSpawnTime = Time.time;
    }

    private IEnumerator ExpandBomb(GameObject bomb)
    {
        float elapsedTime = 0f;
        float initialSize = bomb.transform.localScale.x;
        float targetSize = initialSize * 3f;

        CircleCollider2D circleCol = bomb.GetComponent<CircleCollider2D>();

        while (elapsedTime < 1.5f)
        {
            float newSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / 1.5f);
            bomb.transform.localScale = Vector3.one * newSize;

            circleCol.radius = newSize / 2f;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(bomb.transform.position, targetSize / 2f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(25);
                }
            }
        }

        Destroy(bomb);
    }

    private void FleeDirection()
    {
        Vector2 directionToTarget = (_player.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -directionToTarget, _detectionRadius, _colliders);

        if (hit.collider != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            transform.Translate(randomDirection * _speed * Time.deltaTime);

            UpdateAnimations(randomDirection);
        }
        else
        {
            transform.Translate(-directionToTarget * _speed * Time.deltaTime);

            UpdateAnimations(-directionToTarget);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isFleeing = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isFleeing = false;
    }

    private void FixedUpdate()
    {
        if (!_canSpawnBomb && Time.time - _lastBombSpawnTime >= _bombCooldown)
        {
            _canSpawnBomb = true;
        }
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
}
