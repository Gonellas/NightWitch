using UnityEngine;

public class FireAttack : Attacks
{
    public GameObject firePrefab;
    public Transform fireSpawn;
    protected override void PerformAttack(Direction direction)
    {
        if(direction == Direction.Right)
        {
            Bullet fire = BulletFactory.Instance.GetObjectFromPool();

            fire.transform.position = fireSpawn.position;
            fire.gameObject.SetActive(true);

            EnemyDamaged(25);
        }

    }
}
