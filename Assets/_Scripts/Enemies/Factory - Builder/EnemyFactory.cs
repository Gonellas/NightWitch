using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }
    
    [SerializeField] private Enemy _fairyPrefab;
    [SerializeField] private Enemy _zombiePrefab;
    [SerializeField] private int initialAmount;

    private Pool<Enemy> _fairyPool;
    private Pool<Enemy> _zombiePool;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _zombiePool = new Pool<Enemy>(CreateZombie, Enemy.TurnOn, Enemy.TurnOff, initialAmount);
        _fairyPool = new Pool<Enemy>(CreateFairy, Enemy.TurnOn, Enemy.TurnOff, initialAmount);
        
    }
    
    Enemy CreateZombie()
    {
        return Instantiate(_zombiePrefab);
    }

    Enemy CreateFairy()
    {
        return Instantiate(_fairyPrefab);
    }

    public Enemy GetObjectFromPool(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Zombie:
                return _zombiePool.GetObject();
            case EnemyType.Fairy:
                return _fairyPool.GetObject();
            default:
                return null;
        }
    }
    
    public void ReturnObjectToPool(Enemy enemy)
    {
        switch (enemy.EnemyType)
        {
            case EnemyType.Zombie:
                _zombiePool.ReturnObjectToPool(enemy);
                break;
            case EnemyType.Fairy:
                _zombiePool.ReturnObjectToPool(enemy);
                break;
        }
    }
}
