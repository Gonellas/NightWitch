using System;
using UnityEngine;

public class FireAttack : Swipe
{
    [SerializeField] private GameObject _fireBullet;

    public FireAttack(Transform transform, GameObject trail, GameObject fireBullet) : base(transform, trail)
    {
        _fireBullet = fireBullet;
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
        Debug.Log("Fire attack!");
    }
}

