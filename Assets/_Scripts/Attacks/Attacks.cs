using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attacks : MonoBehaviour
{
    [SerializeField] private GameObject trail;
    [SerializeField] private float radius = 1f;
    private Vector2 initialTouch;
    private Vector2 finalTouch;

    private GameObject currentTrail;

    public virtual void AttackController()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(Input.touchCount - 1);

            if (touch.position.x >= Screen.width * 0.5f && touch.position.y <= Screen.height * 0.5f)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    initialTouch = touch.position;

                    if (currentTrail != null)
                    {
                        Destroy(currentTrail);
                    }

                    currentTrail = Instantiate(trail);
                    currentTrail.transform.position = touch.position;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    currentTrail.transform.position = touch.position;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    finalTouch = touch.position;

                    Vector2 swipeDirection = (finalTouch - initialTouch);

                    if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                    {
                        if (swipeDirection.x > 0)
                        {
                            Debug.Log("Swiped right");
                            PerformAttack(Direction.Right);
                        }
                        else
                        {
                            Debug.Log("Swiped left");
                            PerformAttack(Direction.Left);
                        }
                    }
                    else
                    {
                        // Vertical swipe
                        if (swipeDirection.y > 0)
                        {
                            Debug.Log("Swiped up");
                            PerformAttack(Direction.Up);
                        }
                        else
                        {
                            Debug.Log("Swiped down");
                            PerformAttack(Direction.Down);
                        }
                    }

                    Destroy(currentTrail);
                }
            }
        }
    }

    protected abstract void PerformAttack(Direction direction);

    protected enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    protected virtual void EnemyDamaged(int damage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach(Collider2D col in colliders)
        {
            IDamageable damageable = col.GetComponent<IDamageable>();

            if(damageable != null && col.gameObject != gameObject)
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
