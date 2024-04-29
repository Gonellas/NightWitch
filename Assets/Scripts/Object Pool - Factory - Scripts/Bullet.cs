using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _initialLifeTime;
    private float _currentLifeTime;
    [SerializeField] private float _speed;
    private GameObject enemy;
    public Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy"); // = SelectedEnemy
    }

    void Update()
    {
        if (player != null && player.ClosestEnemy != null)
        {
            Vector3 moveDir = (player.ClosestEnemy.transform.position - transform.position).normalized;
            transform.position += moveDir * _speed * Time.deltaTime;
        }

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