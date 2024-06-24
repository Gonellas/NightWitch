using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack : Swipe
{
    [SerializeField] private GameObject _groundBullet;

    public GroundAttack(Transform transform, GameObject trail, GameObject fireBullet) : base(transform, trail)
    {
        _groundBullet = fireBullet;
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
        Debug.Log("Ground attack!");
    }
}
