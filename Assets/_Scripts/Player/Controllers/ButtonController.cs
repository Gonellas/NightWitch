using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : Controller
{
    public override Vector2 GetMovementInput()
    {
        return _moveDir;
    }

    public void MoveUp() 
    {
        _moveDir = Vector2.up;
    }
    public void MoveRight() 
    {
        _moveDir = Vector2.right;
    }
    public void MoveLeft() 
    { 
        _moveDir = Vector2.left;

    }
    public void MoveDown() 
    { 
        _moveDir = Vector2.down;

    }
    public void Static() 
    {
        _moveDir = Vector2.zero;
    }
}
