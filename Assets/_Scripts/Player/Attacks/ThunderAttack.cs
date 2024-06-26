using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttack : Swipe
{
    [SerializeField] private GameObject _thunderBullet;

    public ThunderAttack(Transform transform, GameObject trail, GameObject thunderBullet) : base(transform, trail)
    {
        _thunderBullet = thunderBullet;
    }

    public override Vector2 SwipeDetection()
    {
        Vector2 swipeDirection = base.SwipeDetection();
        if (swipeDirection != Vector2.zero)
        {
            //EventManager.TriggerEvent(EventsType.Thunder_Attack, swipeDirection);
            CreateThunderEffect();
        }
        return swipeDirection;
    }

    private void CreateThunderEffect()
    {
        var bullet = BulletFactory.Instance.GetObjectFromPool(BulletType.Thunder);
        bullet.transform.position = _transform.position;
        Debug.Log("Thunder attack!");
    }
}
