using UnityEngine;

public abstract class EnemyDetector : MonoBehaviour
{

    private Enemy closestEnemy = null;
    public Enemy ClosestEnemy { get; private set; }

    [SerializeField] private Material enemyMaterial;
    private void Start()
    {
        // Buscar el jugador en la escena
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    public void FindClosestEnemy(Vector2 playerPosition)
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            if (currentEnemy != null && currentEnemy.gameObject.activeInHierarchy)
            {
                Vector2 enemyPosition = currentEnemy.transform.position; // Posición del enemigo en 2D
                float distanceToEnemy = (enemyPosition - playerPosition).sqrMagnitude;
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
        }

        ClosestEnemy = closestEnemy;
    }
}