using System.Collections;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _bomb; 
    [SerializeField] float _speed = 5f;
    [SerializeField] float _detectionRadius = 5f;
    [SerializeField] LayerMask _colliders;

    private bool _canSpawnBomb = true;
    private bool _isFleeing = false; 
    private float _lastBombSpawnTime; 
    private float _bombCooldown = 10f; 

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

        if (_isFleeing)
        {
            FleeDirection();
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
        Vector3 initialScale = bomb.transform.localScale;
        Vector3 targetScale = initialScale * 3f; 

        while (elapsedTime < 1.5f) 
        {
          
            bomb.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / 1.5f);

            elapsedTime += Time.deltaTime;

            yield return null;
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
        }
        else
        {
            transform.Translate(-directionToTarget * _speed * Time.deltaTime);
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
}
