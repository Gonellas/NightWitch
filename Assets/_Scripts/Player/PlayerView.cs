using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private Animator _playerAnim;

    private void Start()
    {
        if (_playerAnim == null) _playerAnim.GetComponentInChildren<Animator>();
    }

}
