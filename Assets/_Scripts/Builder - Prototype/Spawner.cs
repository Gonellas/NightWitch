using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float interval;
    private float timer = 0f;

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
                                .SetColor(Random.ColorHSV())
                                .SetPosition(Random.Range(-5, 5f), Random.Range(-5, 5f), Random.Range(-5, 5f))
                                .SetMaxLife(Random.Range(50, 101))
                                .Done();
            }
            else
            {
                var enemy = new EnemyBuilder(EnemyFactory.Instance.GetZombieFromPool)
                                .SetColor(Random.ColorHSV())
                                .SetPosition(Random.Range(-5, 5f), Random.Range(-5, 5f), Random.Range(-5, 5f))
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

