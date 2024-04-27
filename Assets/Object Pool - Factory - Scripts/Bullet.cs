using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _initialLifeTime;

    private float _currentLifeTime;


    void Update()
    {
        _currentLifeTime -= Time.deltaTime;

        if (_currentLifeTime > 0) return;
        
        BulletFactory.Instance.ReturnObjectToPool(this);
    }

    private void Reset()
    {
        _currentLifeTime = _initialLifeTime;
    }

    public static void TurnOn(Bullet b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Bullet b)
    {
        b.gameObject.SetActive(false);
    }
}
