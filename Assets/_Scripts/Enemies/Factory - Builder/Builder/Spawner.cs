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
            timer = 0f;
            float randomNumber = Random.Range(0f, 1f);


            if (randomNumber < 0.5f)
            {
                var zombie = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Zombie);
                if (zombie != null)
                {
                    new EnemyBuilder(() => zombie)
                        .SetColor(colors[Random.Range(0, colors.Length)])
                        .SetPosition(Random.Range(-27, 25f), Random.Range(-9, 9f), 0)
                        .SetMaxLife(Random.Range(50, 101))
                        .Done();
                }
                else
                {
                    Debug.LogError("Failed to get a zombie from the pool.");
                }
            }
            else
            {
                var fairy = EnemyFactory.Instance.GetObjectFromPool(EnemyType.Fairy);
                if (fairy != null)
                {
                    new EnemyBuilder(() => fairy)
                        .SetColor(colors[Random.Range(0, colors.Length)])
                        .SetPosition(Random.Range(-27, 25f), Random.Range(-9, 9f), 0)
                        .SetMaxLife(Random.Range(0, 101))
                        .Done();
                }
                else
                {
                    Debug.LogError("Failed to get a fairy from the pool.");
                }

            }
        }
    }
}