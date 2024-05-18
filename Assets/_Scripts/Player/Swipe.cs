using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : AttackController
{
    private Vector2 initialTouch;
    private Vector2 finalTouch;

    [SerializeField] private GameObject trail;
    private GameObject currentTrail;

    public override Vector2 SwipeDetection()
    {
        if (!GameManager.instance.IsPaused())
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

                        Vector2 swipeDirection = finalTouch - initialTouch;

                        Destroy(currentTrail);

                        return swipeDirection;
                    }
                }
            }
        }
        return Vector2.zero;
    }
}