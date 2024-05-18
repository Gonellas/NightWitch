using UnityEngine;

public class FireAttack : Swipe
{
    [SerializeField] private GameObject fireEffect;

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
        Instantiate(fireEffect, transform.position, Quaternion.identity);
        Debug.Log("Fire attack!");
    }
}
