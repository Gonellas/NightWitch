using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Animator")]
    Animator _animator;
    private Vector2 _lastMovement = Vector2.zero;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        EventManager.SubscribeToEvent(EventsType.Movement, OnMove);

        //Attacks
        EventManager.SubscribeToEvent(EventsType.Fire_Attack, OnFireAttack);
        EventManager.SubscribeToEvent(EventsType.Ice_Attack, OnIceAttack);
        EventManager.SubscribeToEvent(EventsType.Ground_Attack, OnGroundAttack);
        EventManager.SubscribeToEvent(EventsType.Thunder_Attack, OnThunderAttack);

        //PowerUps
        
    }

    private void OnDestroy()
    {
        EventManager.UnsubscribeToEvent(EventsType.Movement, OnMove);

        //Attacks
        EventManager.UnsubscribeToEvent(EventsType.Fire_Attack, OnFireAttack);
        EventManager.UnsubscribeToEvent(EventsType.Ground_Attack, OnGroundAttack);
        EventManager.UnsubscribeToEvent(EventsType.Thunder_Attack, OnThunderAttack);
        EventManager.UnsubscribeToEvent(EventsType.Ice_Attack, OnIceAttack);

        //PowerUps
    }
    #region Movement Anim
    private void OnMove(params object[] parameters)
    {
        if (parameters.Length > 0 & parameters[0] is Vector2)
        {
            Vector2 movement = (Vector2)parameters[0];
            UpdateAnimations(movement);
        }
    }
    #endregion

    #region Attack Anims
    private void OnFireAttack(params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0 && parameters[0] is Vector2)
        {
            Vector2 direction = (Vector2)parameters[0];
            UpdateAttackAnimations(direction, "FireAttack");
        }
        else
        {
            Debug.LogWarning("OnFireAttack: Invalid parameters received.");
        }
    }
    private void OnIceAttack(params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0 & parameters[0] is Vector2)
        {
            Vector2 dir = (Vector2)parameters[0];
            UpdateAttackAnimations(dir, "IceAttack");
        }
    }
    private void OnGroundAttack(params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0 & parameters[0] is Vector2)
        {
            Vector2 dir = (Vector2)parameters[0];
            UpdateAttackAnimations(dir, "GroundAttack");
        }
    }
    private void OnThunderAttack(params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0 & parameters[0] is Vector2)
        {
            Vector2 dir = (Vector2)parameters[0];
            UpdateAttackAnimations(dir, "ThunderAttack");
        }
    }
    #endregion

    #region Animations Update
    private void UpdateAnimations(Vector2 movement)
    {
        if (!GameManager.instance.IsPaused())
        {
            if (movement.magnitude > 0)
            {
                _animator.SetBool("isWalking", true);
                _animator.SetFloat("HAx", movement.x);
                _animator.SetFloat("VAx", movement.y);
            }
            else
            {
                _animator.SetBool("isWalking", false);
                _animator.SetFloat("HAx", _lastMovement.x);
                _animator.SetFloat("VAx", _lastMovement.y);
            }

            if (movement.magnitude > 0)
            {
                _lastMovement = movement;
            }
        }
    }

    private void UpdateAttackAnimations(Vector2 dir, string trigger)
    {
        if (!GameManager.instance.IsPaused())
        {
            if (dir.magnitude > 0)
            {
                _animator.SetBool("isAttacking", true);
                _animator.SetFloat("HAx", dir.x);
                _animator.SetFloat("VAx", dir.y);
                _animator.SetTrigger(trigger);
            }
            else
            {
                _animator.SetBool("isAttacking", false);
                Debug.Log("se termino anim ataque");
            }
        }
    }
    #endregion

    public void WalkingR()
    {
        AudioManager.Instance.PlaySFX(SoundType.Player_MovementR, 1f);
    }
    public void WalkingL()
    {
        AudioManager.Instance.PlaySFX(SoundType.Player_MovementL, 1f);
    }
}
