using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] Player player;

    public void FireBall()
    {
        var bullet = BulletFactory.Instance.GetObjectFromPool();
        bullet.transform.position = player.transform.position;
    }
}

