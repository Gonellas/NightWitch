using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttack : Swipe
{
    [SerializeField] private GameObject _iceBullet;

    public IceAttack(Transform transform, GameObject trail, GameObject iceBullet) : base(transform, trail)
    {
        _iceBullet = iceBullet;
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
        AudioManager.instance.PlaySFX(SoundType.Ice, 1f);
        var bullet = BulletFactory.Instance.GetObjectFromPool(BulletType.Ice);
        bullet.transform.position = _transform.position;
        Debug.Log("Ice attack!");
    }
}
