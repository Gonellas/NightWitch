using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Controller _controller;
    [SerializeField] float _speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(_controller.GetMovementInput().x, _controller.GetMovementInput().y, 0) * _speed * Time.deltaTime;
    }
    
}
