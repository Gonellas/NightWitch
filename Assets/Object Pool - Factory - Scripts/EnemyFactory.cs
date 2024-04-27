using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }
    
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int initialAmount;
    
    private Pool<Enemy> _enemyPool;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _enemyPool = new Pool<Enemy>(() => Instantiate(_enemyPrefab), 
                                (e) => e.gameObject.SetActive(true) , 
                                (e) => e.gameObject.SetActive(false), 
                                initialAmount);
    }
    
    public Enemy GetObjectFromPool()
    {
        return _enemyPool.GetObject();
    }

    public void ReturnObjectToPool(Enemy obj)
    {
        _enemyPool.ReturnObjectToPool(obj);
    }
}
