using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance { get; private set; }
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int initialAmount;
    
    private Pool<Bullet> _bulletPool;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _bulletPool = new Pool<Bullet>(CreateObject, Bullet.TurnOn, Bullet.TurnOff, initialAmount);
        
        // _bulletPool = new Pool<Bullet>(()=>Instantiate(_bulletPrefab), 
        //                                 (bullet) => bullet.gameObject.SetActive(true), 
        //                                 (bullet) => bullet.gameObject.SetActive(false), 
        //                                 initialAmount);
    }

    Bullet CreateObject()
    {
        return Instantiate(_bulletPrefab);
    }

    public Bullet GetObjectFromPool()
    {
        return _bulletPool.GetObject();
    }

    public void ReturnObjectToPool(Bullet obj)
    {
        _bulletPool.ReturnObjectToPool(obj);
    }
}