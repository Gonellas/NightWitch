
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : Controller, IDragHandler, IEndDragHandler
{
    [SerializeField, Range(75, 150)] float _maxMagnitude = 125f;
    Vector2 _initialPos;

    //private Animator _animator;

    public delegate void OnMove(Vector2 movement);
    public static event OnMove MoveEvent;

    private void Start()
    {
        _initialPos = transform.position;
    }

    public override Vector2 GetMovementInput()
    {
        Vector3 modifiedDir = new Vector3(_moveDir.x, _moveDir.y, 0);
        modifiedDir /= _maxMagnitude;

        return modifiedDir;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _moveDir = Vector2.ClampMagnitude(eventData.position - _initialPos, _maxMagnitude);
        transform.position = _initialPos + _moveDir;

        MoveEvent?.Invoke(_moveDir);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _initialPos;
        _moveDir = Vector2.zero;

        MoveEvent?.Invoke(Vector2.zero);

    }

}
