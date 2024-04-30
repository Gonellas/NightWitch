using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    private bool movingRight = true; // borrable
    private float timer = 0f;// borrable
    private float speed = 1f;// borrable

    [SerializeField] private float _hp;

    public void LoseHP(float damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

        // borrable todo lo que esta en el update 
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spell1")
        {
            LoseHP(10);
        }
    }
}
