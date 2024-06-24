using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttack : Swipe
{
    [SerializeField] private GameObject _thunderBullet;

    public ThunderAttack(Transform transform, GameObject trail, GameObject fireBullet) : base(transform, trail)
    {
        _thunderBullet = fireBullet;
    }

    public override Vector2 SwipeDetection()
    {
        Vector2 swipeDirection = base.SwipeDetection();
        if (swipeDirection != Vector2.zero)
        {
            CreateFireEffect();
        }
        return swipeDirection;
    }

    private void CreateFireEffect()
    {
        var bullet = BulletFactory.Instance.GetObjectFromPool();
        bullet.transform.position = _transform.position;
        Debug.Log("Thunder attack!");
    }
}
