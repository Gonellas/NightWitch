using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Timing")]
    [SerializeField] private float interval = 3f;
    private float timer = 0f;

    [Header("Enemy Color Options")]
    [SerializeField] private Color color1 = Color.cyan;
    [SerializeField] private Color color2 = Color.white;
    [SerializeField] private Color color3 = Color.yellow;
    private Color[] colors;

    [Header("Spawn Area (World Coordinates)")]
    [SerializeField] private Vector2 spawnMin = new Vector2(-10, -5);
    [SerializeField] private Vector2 spawnMax = new Vector2(10, 5);

    [Header("Enemy Limit")]
    [SerializeField] private int maxEnemies = 10;
    private int currentEnemies = 0;

    private void OnEnable()
    {
        Enemy.OnEnemyDied += EnemyDiedHandler;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= EnemyDiedHandler;
    }

    private void Start()
    {
        colors = new Color[] { color1, color2, color3 };
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval && currentEnemies < maxEnemies)
        {
            timer = 0f;
            SpawnRandomEnemy();
        }
    }

    private void SpawnRandomEnemy()
    {
        float randomNumber = Random.Range(0f, 1f);
        Enemy enemy = null;

        if (randomNumber < 0.33f)
            enemy = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Zombie);
        else if (randomNumber < 0.66f)
            enemy = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Fairy);
        else
            enemy = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Dasher);

        if (enemy != null)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y),
                0
            );

            new EnemyBuilder(() => enemy)
                .SetColor(colors[Random.Range(0, colors.Length)])
                .SetPosition(spawnPos)
                .SetMaxLife(Random.Range(50, 101))
                .Done();

            currentEnemies++;
        }
        else
        {
            Debug.LogWarning("No enemy returned from pool.");
        }
    }

    private void EnemyDiedHandler()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }

    // Para visualizar el área de spawn en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3(
            (spawnMin.x + spawnMax.x) / 2,
            (spawnMin.y + spawnMax.y) / 2,
            0
        );
        Vector3 size = new Vector3(
            Mathf.Abs(spawnMax.x - spawnMin.x),
            Mathf.Abs(spawnMax.y - spawnMin.y),
            0
        );
        Gizmos.DrawWireCube(center, size);
    }
}
