using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _floorMask;

    [SerializeField] private Material enemyMaterial;

    private Enemy closestEnemy = null;

    public Enemy ClosestEnemy { get; private set; }

    JoystickController joystickController; // sirve?

    private Vector2 initialTouch;
    private Vector2 finalTouch;

    [SerializeField] private GameObject trail;
    private GameObject currentTrail;



    void Update()
    {
        transform.position += new Vector3(_controller.GetMovementInput().x, _controller.GetMovementInput().y, 0) * _speed * Time.deltaTime;
        FindClosestEnemy();
        SwipeDetection();
    }

    private void SwipeDetection()
    {

        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(Input.touchCount - 1);

            if (touch.position.x >= Screen.width * 0.5f && touch.position.y <= Screen.height * 0.5f)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    initialTouch = touch.position;
                    
                    if(currentTrail != null)
                    {
                        Destroy(currentTrail);

                    }
                    currentTrail = Instantiate(trail);

                    currentTrail.transform.position = touch.position;

                }
                if(touch.phase == TouchPhase.Moved)
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
                        }
                        else
                        {
                            Debug.Log("Swiped left");
                        }
                    }
                    else
                    {
                        // Vertical swipe
                        if (swipeDirection.y > 0)
                        {
                            Debug.Log("Swiped up");
                        }
                        else
                        {
                            Debug.Log("Swiped down");
                        }
                    }

                    Destroy(currentTrail);
                }
            }
        }

       
    }

    public void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                if (closestEnemy)
                {
                    var renderer2 = closestEnemy.GetComponent<MeshRenderer>();
                    renderer2.material = enemyMaterial;
                }

                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;

                var renderer = currentEnemy.GetComponent<MeshRenderer>();
                renderer.material = default;
            }
        }

        ClosestEnemy = closestEnemy;
    }
}