using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack : Swipe
{
    [SerializeField] private GameObject _groundBullet;

    public GroundAttack(Transform transform, GameObject trail, GameObject groundBullet) : base(transform, trail)
    {
        _groundBullet = groundBullet;
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
        var bullet = BulletFactory.Instance.GetObjectFromPool(BulletType.Ground);
        bullet.transform.position = _transform.position;
        Debug.Log("Ground attack!");
    }
}
