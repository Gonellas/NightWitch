using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _floorMask;

    public Enemy ClosestEnemy { get; private set; }

    JoystickController joystickController;

    void Update()
    {
        transform.position += new Vector3(_controller.GetMovementInput().x, _controller.GetMovementInput().y, 0) * _speed * Time.deltaTime;
        FindClosestEnemy();
        FireBall();
    }

    private void FireBall()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touchCount <= 0) return;

            Touch currentTouch = Input.touches[i];

            if (currentTouch.phase == TouchPhase.Began)
            {
                var bullet = BulletFactory.Instance.GetObjectFromPool();
                bullet.transform.position = transform.position;
            }
        }
    }

    public void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        ClosestEnemy = closestEnemy; // Set the closest enemy reference
    }
}