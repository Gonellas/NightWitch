using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float interval;
    private float timer = 0f;

    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] Color color3;

    [SerializeField] Color[] colors;

    [SerializeField] private int maxEnemies = 10;  // Máximo de enemigos activos permitidos
    private int currentEnemies = 0;

    private void OnEnable()
    {
        Enemy.OnEnemyDied += EnemyDiedHandler;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= EnemyDiedHandler;
    }

    private void EnemyDiedHandler()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }

    private void Start()
    {
        color1 = Color.cyan;
        color2 = Color.white;
        color3 = Color.yellow;

        colors = new Color[] { color1, color2, color3 };
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if (currentEnemies >= maxEnemies)
                return;

            timer = 0f;
            float randomNumber = Random.Range(0f, 1f);

            if (randomNumber < 0.33f)
            {
                var zombie = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Zombie);
                if (zombie != null)
                {
                    new EnemyBuilder(() => zombie)
                        .SetColor(colors[Random.Range(0, colors.Length)])
                        .SetPosition(Random.Range(-22f, 1f), Random.Range(-9f, 9f), 0)
                        .SetMaxLife(Random.Range(50, 101))
                        .Done();

                    currentEnemies++;
                }
                else
                {
                    Debug.LogError("Failed to get a zombie from the pool.");
                }
            }
            else if (randomNumber < 0.66f)
            {
                var fairy = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Fairy);
                if (fairy != null)
                {
                    new EnemyBuilder(() => fairy)
                        .SetColor(colors[Random.Range(0, colors.Length)])
                        .SetPosition(Random.Range(-22f, 1f), Random.Range(-9f, 9f), 0)
                        .SetMaxLife(Random.Range(30, 81))
                        .Done();

                    currentEnemies++;
                }
                else
                {
                    Debug.LogError("Failed to get a fairy from the pool.");
                }
            }
            else
            {
                var dasher = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Dasher);
                if (dasher != null)
                {
                    new EnemyBuilder(() => dasher)
                        .SetColor(colors[Random.Range(0, colors.Length)])
                        .SetPosition(Random.Range(-22f, 1f), Random.Range(-9f, 9f), 0)
                        .SetMaxLife(Random.Range(60, 121))
                        .Done();

                    currentEnemies++;
                }
                else
                {
                    Debug.LogError("Failed to get a dasher from the pool.");
                }
            }
        }
    }
}
