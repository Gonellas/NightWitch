using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilder
{
    public Func<Enemy> _instantiateMethod;

    Vector3 _newPosition;
    //Vector3 _newScale;
    Color _newColor;

    float _newMaxLife;

    public EnemyBuilder(Func<Enemy> factoryMethod)
    {
        _newColor = Color.white;

        _instantiateMethod = factoryMethod;
        _newMaxLife = 100;
    }

    #region Position

    public EnemyBuilder SetPosition(float x, float y, float z)
    {
        return SetPosition(new Vector3(x, y, z));
    }

    public EnemyBuilder SetPosition(Vector3 pos)
    {
        _newPosition = pos;
        return this;
    }

    #endregion

    #region Scale

    //public EnemyBuilder SetScale(Vector3 s)
    //{
    //    _newScale = s;
    //    return this;
    //}

    #endregion

    #region Color
    public EnemyBuilder SetColor(Color c)
    {
        _newColor = c;
        return this;
    }
    #endregion

    #region Max Life
    public EnemyBuilder SetMaxLife(float l)
    {
        _newMaxLife = l;
        return this;
    }
    #endregion

    public Enemy Done()
    {
        var e = _instantiateMethod();

        e.transform.position = _newPosition;
        //e.transform.localScale = _newScale;
        e.GetComponent<SpriteRenderer>().color = _newColor;

        e.hp = _newMaxLife;

        e.instantiateMethod = _instantiateMethod;

        return e;
    }
}
