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
            CreateThunderEffect();
        }
        return swipeDirection;
    }

    private void CreateThunderEffect()
    {
        AudioManager.instance.PlaySFX(SoundType.Thunder, 1f);
        var bullet = BulletFactory.Instance.GetObjectFromPool(BulletType.Thunder);
        bullet.transform.position = _transform.position;
        Debug.Log("Thunder attack!");
    }
}
