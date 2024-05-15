using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _initialLifeTime;
    [SerializeField] private float _speed;
    private float _currentLifeTime;
    private Enemy targetEnemy;
    private EnemyDetector enemyDetector;

    void Start()
    {
        enemyDetector = FindObjectOfType<EnemyDetector>();
    }

    void Update()
    {
        if (enemyDetector != null)
        {
            targetEnemy = enemyDetector.ClosestEnemy;
        }

        if (targetEnemy != null)
        {
            Vector3 moveDir = (targetEnemy.transform.position - transform.position).normalized;
            transform.position += moveDir * _speed * Time.deltaTime;
        }

        _currentLifeTime -= Time.deltaTime;

        if (_currentLifeTime <= 0)
        {
            BulletFactory.Instance.ReturnObjectToPool(this);
        }
    }

    private void Reset()
    {
        _currentLifeTime = _initialLifeTime;
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