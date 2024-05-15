using UnityEngine;
public class EnemyDetector : MonoBehaviour
{
    private Enemy closestEnemy;
    public Enemy ClosestEnemy { get; private set; }
    [SerializeField] float _detectionRadius = 10f;
    [SerializeField] private Material enemyMaterial;

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

                if(distanceToEnemy < distanceToClosestEnemy)
                {
                    Debug.Log("enemy en el radio");
                    if (closestEnemy)
                    {
                        Debug.Log("Enemigo cerca" + enemy.gameObject);
                        //var renderer2 = closestEnemy.GetComponent<MeshRenderer>();
                        //renderer2.material = enemyMaterial;
                    }

                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = enemy.GetComponent<Enemy>();

                    Debug.Log("Enemigo no cerca" + enemy.gameObject);

                    //var renderer = closestEnemy.GetComponent<MeshRenderer>();
                    //renderer.material = default;
                }
            }
        }

        ClosestEnemy = closestEnemy;
    }
}