using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private Enemy closestEnemy;
    public Enemy ClosestEnemy { get; private set; }
    [SerializeField] float _detectionRadius = 10f;

    private void Update()
    {
        FindClosestEnemy(transform.position);
    }

    private void Start()
    {
        FindClosestEnemy(transform.position);
    }

    public void FindClosestEnemy(Vector2 playerPosition)
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _detectionRadius);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                Vector2 enemyPosition = enemy.transform.position;
                float distanceToEnemy = (enemyPosition - (Vector2)transform.position).sqrMagnitude;

                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = enemy.GetComponent<Enemy>();
                }
            }
        }
        ClosestEnemy = closestEnemy;
    }

    public Vector2 GetDirectionToClosestEnemy(Vector2 playerPosition)
    {
        if (ClosestEnemy != null)
        {
            Vector2 enemyPosition = ClosestEnemy.transform.position;
            return (enemyPosition - playerPosition).normalized;
        }
        return Vector2.zero;
    }
}
