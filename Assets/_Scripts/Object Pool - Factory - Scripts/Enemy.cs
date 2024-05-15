using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
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
