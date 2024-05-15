using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _initialLifeTime;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage = 10f;
    private Enemy _targetEnemy;
    private EnemyDetector _enemyDetector;


    void Start()
    {
        _enemyDetector = FindObjectOfType<EnemyDetector>();
        if (_enemyDetector == null)
        {
            Debug.LogWarning("No se detectó ningún enemigo.");
        }
        Reset();
    }

    void Update()
    {
        if (_targetEnemy == null)
        {
            if (_enemyDetector != null)
            {
                _targetEnemy = _enemyDetector.ClosestEnemy;
            }
        }
        else
        {
            MoveBulletToTarget();
        }

        _initialLifeTime -= Time.deltaTime;

        if (_initialLifeTime <= 0)
        {
            BulletFactory.Instance.ReturnObjectToPool(this);
        }
    }

    public void SetTargetEnemy(Enemy target)
    {
        _targetEnemy = target;
    }

    private void MoveBulletToTarget()
    {
        if (_targetEnemy == null) return;

        Vector3 moveDir = (_targetEnemy.transform.position - transform.position).normalized;
        transform.position += moveDir * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _targetEnemy.LoseHP(_damage);
            BulletFactory.Instance.ReturnObjectToPool(this);

        }
        else if (!other.gameObject.CompareTag("Player")) 
        {
            BulletFactory.Instance.ReturnObjectToPool(this);
        }
    }

    private void Reset()
    {
        _initialLifeTime = 2f;
        _targetEnemy = null;
    }


    public static void TurnOn(Bullet b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Bullet b)
    {
        b.gameObject.SetActive(false);
    }
}