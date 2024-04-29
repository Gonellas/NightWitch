using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool movingRight = true;
    private float timer = 0f;
    private float speed = 1f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= 1f)
        {
            if (movingRight)
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            else
                transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        else if (timer <= 2f)
        {
            if (movingRight)
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            else
                transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else
        {
            timer = 0f;
            movingRight = !movingRight;
        }
    }
}
