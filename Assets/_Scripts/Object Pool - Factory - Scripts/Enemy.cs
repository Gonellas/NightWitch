using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour, IEnemy
{

    public Func<Enemy> instantiateMethod;
    public float hp;
    public abstract Vector3 GetPosition();
    protected abstract void SteeringBehaviour();
    [SerializeField] GameObject _coin;
    public void LoseHP(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (hp <= 0)
        {
            Instantiate(_coin, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
