
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
        //_animator = GetComponent<Animator>();
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
        
        //UpdateAnimations();

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _initialPos;
        _moveDir = Vector2.zero;

        MoveEvent?.Invoke(Vector2.zero);

        //UpdateAnimations();
    }

    //private void UpdateAnimations()
    //{
    //    float angle = Vector2.SignedAngle(Vector2.up, _moveDir);

    //    if(angle >= -45 && angle < 45)
    //    {
    //        _animator.SetFloat("HAx", 1f);
    //        _animator.SetFloat("VAx", 0f);
    //    }
    //    else if(angle >= 45 && angle < 135)
    //    {
    //        _animator.SetFloat("HAx", 0f);
    //        _animator.SetFloat("VAx", 1f);
    //    }
    //    else if (angle >= -135 && angle < -45)
    //    {
    //        _animator.SetFloat("HAx", 0f);
    //        _animator.SetFloat("VAx", -1f);
    //    }
    //    else
    //    {
    //        _animator.SetFloat("HAx", -1f);
    //        _animator.SetFloat("VAx", 0f);
    //    }
    //}
}
