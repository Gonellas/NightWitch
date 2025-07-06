using System;
using UnityEngine;

public class EnemyBuilder
{
    public Func<Enemy> _instantiateMethod;

    Vector3 _newPosition;
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

    #region Color

    public EnemyBuilder SetColor(Color c)
    {
        // 🔧 Aseguramos que el color tenga alpha completo (visible)
        c.a = 1f;
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

        if (e == null)
        {
            Debug.LogError("Failed to instantiate enemy.");
            return null;
        }

        e.transform.position = _newPosition;
        e.GetComponent<SpriteRenderer>().color = _newColor;
        e.hp = _newMaxLife;
        e.instantiateMethod = _instantiateMethod;

        return e;
    }
}
