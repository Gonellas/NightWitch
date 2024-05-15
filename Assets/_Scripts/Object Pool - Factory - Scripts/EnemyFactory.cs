using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }
    
    [SerializeField] private Fairy _fairyPrefab;
    [SerializeField] private Zombie _zombiePrefab;
    [SerializeField] private int initialAmount;

    private Pool<Fairy> _fairyPool;
    private Pool<Zombie> _zombiePool;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _fairyPool = new Pool<Fairy>(() => Instantiate(_fairyPrefab), 
                                (e) => e.gameObject.SetActive(true) , 
                                (e) => e.gameObject.SetActive(false), 
                                initialAmount);

        _zombiePool = new Pool<Zombie>(() => Instantiate(_zombiePrefab),
                        (e) => e.gameObject.SetActive(true),
                        (e) => e.gameObject.SetActive(false),
                        initialAmount);
    }
    
    public Fairy GetFairyFromPool()
    {
        return _fairyPool.GetObject();
    }

    public void ReturnFairyToPool(Fairy obj)
    {
        _fairyPool.ReturnObjectToPool(obj);
    }

    public Zombie GetZombieFromPool()
    {
        return _zombiePool.GetObject();
    }

    public void ReturnZombieToPool(Zombie obj)
    {
        _zombiePool.ReturnObjectToPool(obj);
    }
}
