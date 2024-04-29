using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _floorMask;

    JoystickController joystickController;

    private enum ScreenQuadrant { TopLeft, TopRight, BottomLeft, BottomRight }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(_controller.GetMovementInput().x, _controller.GetMovementInput().y, 0) * _speed * Time.deltaTime;

            FireBall();
    }

    public void FireBall()
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

}