using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance { get; private set; }

    [SerializeField] private Bullet _fireBulletPrefab;
    [SerializeField] private Bullet _iceBulletPrefab;
    [SerializeField] private Bullet _groundBulletPrefab;
    [SerializeField] private Bullet _thunderBulletPrefab;
    [SerializeField] private int initialAmount;

    private Pool<Bullet> _fireBulletPool;
    private Pool<Bullet> _iceBulletPool;
    private Pool<Bullet> _groundBulletPool;
    private Pool<Bullet> _thunderBulletPool;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _fireBulletPool = new Pool<Bullet>(CreateFireBullet, Bullet.TurnOn, Bullet.TurnOff, initialAmount);
        _iceBulletPool = new Pool<Bullet>(CreateIceBullet, Bullet.TurnOn, Bullet.TurnOff, initialAmount);
        _groundBulletPool = new Pool<Bullet>(CreateGroundBullet, Bullet.TurnOn, Bullet.TurnOff, initialAmount);
        _thunderBulletPool = new Pool<Bullet>(CreateThunderBullet, Bullet.TurnOn, Bullet.TurnOff, initialAmount);
    }

    Bullet CreateFireBullet()
    {
        return Instantiate(_fireBulletPrefab);
    }
    Bullet CreateIceBullet()
    {
        return Instantiate(_iceBulletPrefab);
    }
    Bullet CreateGroundBullet()
    {
        return Instantiate(_groundBulletPrefab);
    }
    Bullet CreateThunderBullet()
    {
        return Instantiate(_thunderBulletPrefab);
    }

    public Bullet GetObjectFromPool(BulletType bulletType)
    {
        switch (bulletType)
        {
            case BulletType.Fire:
                return _fireBulletPool.GetObject();
            case BulletType.Ice:
                return _iceBulletPool.GetObject();
            case BulletType.Ground:
                return _groundBulletPool.GetObject();
            case BulletType.Thunder:
                return _thunderBulletPool.GetObject();
            default:
                return null;
        }
    }

    public void ReturnObjectToPool(Bullet bullet)
    {
        switch (bullet.BulletType)
        {
            case BulletType.Fire:
                _fireBulletPool.ReturnObjectToPool(bullet);
                break;
            case BulletType.Ice:
                _iceBulletPool.ReturnObjectToPool(bullet);
                break;
            case BulletType.Ground:
                _groundBulletPool.ReturnObjectToPool(bullet);
                break;
            case BulletType.Thunder:
                _thunderBulletPool.ReturnObjectToPool(bullet);
                break;
        }
    }
}
