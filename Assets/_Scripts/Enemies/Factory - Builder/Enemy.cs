using UnityEngine;
using System;

public enum EnemyType
{
    Zombie,
    Fairy,
    Dasher,
}

public abstract class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] GameObject _coin;
    Enemy enemy;
    public float hp;

    public Func<Enemy> instantiateMethod;

    [SerializeField] private EnemyType _enemyType;
    public EnemyType EnemyType => _enemyType;  

    public abstract Vector3 GetPosition();

    protected abstract void SteeringBehaviour();

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
        AudioManager.instance.PlaySFX(SoundType.Coin, 1f);
        Instantiate(_coin, transform.position, transform.rotation);
        EnemyFactory.Instance.ReturnObjectToPool(this);
    }

    public static void TurnOn(Enemy b)
    {
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Enemy b)
    {
        b.gameObject.SetActive(false);
    }
}
