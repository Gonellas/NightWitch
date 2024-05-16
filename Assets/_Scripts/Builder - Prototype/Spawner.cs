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
                var enemy = new EnemyBuilder(EnemyFactory.Instance.GetFairyFromPool)
                                .SetColor(colors[Random.Range(0, colors.Length)])
                                .SetPosition(Random.Range(-27, 25f), Random.Range(-9, 9f), 0)
                                .SetMaxLife(Random.Range(50, 101))
                                .Done();
            }
            else
            {
                var enemy = new EnemyBuilder(EnemyFactory.Instance.GetZombieFromPool)
                                .SetColor(colors[Random.Range(0, colors.Length)])
                                .SetPosition(Random.Range(-27, 25f), Random.Range(-9, 9f), 0)
                                .SetMaxLife(Random.Range(0, 101))
                                .Done();
            }
        }
    }
}


    //    if (Input.GetKeyDown(KeyCode.E))
    //    {

    //        var enemy = new EnemyBuilder(EnemyFactory.Instance.GetFairyFromPool)
    //                        .SetColor(Random.ColorHSV())
    //                        .SetPosition(Random.Range(-5, 5f), Random.Range(-5, 5f), Random.Range(-5, 5f))
    //                        .SetMaxLife(Random.Range(50, 101))
    //                        .Done();

    //    }
    //    else if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        var enemy = new EnemyBuilder(EnemyFactory.Instance.GetZombieFromPool)
    //                        .SetColor(Random.ColorHSV())
    //                        .SetPosition(Random.Range(-5, 5f), Random.Range(-5, 5f), Random.Range(-5, 5f))
    //                        .SetMaxLife(Random.Range(0, 101))
    //                        .Done();

    //    }
    //}

