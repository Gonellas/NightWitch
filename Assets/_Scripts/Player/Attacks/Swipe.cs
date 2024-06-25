using UnityEngine;

public class Swipe : MonoBehaviour, IAttack
{
    protected Transform _transform;
    protected GameObject _trail;

    private Vector2 _initialTouch;
    private Vector2 _finalTouch;
    private GameObject _currentTrail;

    public Swipe(Transform transform, GameObject trail)
    {
        _transform = transform;
        _trail = trail;
    }

    public virtual Vector2 SwipeDetection()
    {
        if (!GameManager.instance.IsPaused())
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(Input.touchCount - 1);

                if (touch.position.x >= Screen.width * 0.5f && touch.position.y <= Screen.height * 0.5f)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        _initialTouch = touch.position;

                        if (_currentTrail != null)
                        {
                            Destroy(_currentTrail);
                        }
                        _currentTrail = Instantiate(_trail);
                        _currentTrail.transform.position = touch.position;
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        _currentTrail.transform.position = touch.position;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        _finalTouch = touch.position;

                        Vector2 swipeDirection = _finalTouch - _initialTouch;

                        Destroy(_currentTrail);

                        return swipeDirection;
                    }
                }
            }
        }
        return Vector2.zero;
    }
}
