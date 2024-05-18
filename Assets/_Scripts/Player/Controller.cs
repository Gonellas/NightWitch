using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected Vector2 _moveDir;
    public abstract Vector2 GetMovementInput();

}