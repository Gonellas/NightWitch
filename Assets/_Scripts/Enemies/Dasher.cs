using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : Enemy
{
    public float moveSpeed = 2f;      
    public float detectionRange = 5f; 
    public float dashSpeed = 10f;     
    public float dashDistance = 2f;   
    public float attackCooldown = 2f; 
    public int damage = 10;           

    private Transform player;
    private Rigidbody2D rb;
    private bool isDashing = false;
    private bool onCooldown = false;
    private Vector2 dashDirection;
    private Animator _animator;


    protected override void SteeringBehaviour()
    {
        
    }

    public override Vector3 GetPosition()
    {
        return transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {


        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isDashing && !onCooldown)
        {
            if (distanceToPlayer <= detectionRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;

                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                UpdateAnimations(direction);
            }
            else UpdateAnimations(Vector2.zero);

            if (distanceToPlayer <= dashDistance)
            {
                StartDash();
            }
        }
        else UpdateAnimations(Vector2.zero);

    }

    void StartDash()
    {
        isDashing = true;
        dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;
        Invoke("StopDash", 0.2f); 
    }

    void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;
        onCooldown = true;
        Invoke("ResetCooldown", attackCooldown);
    }

    void ResetCooldown()
    {
        onCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDashing && other.CompareTag("Player"))
        {
            
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    void UpdateAnimations(Vector2 movement)
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
            _animator.SetFloat("HAx", movement.x);
            _animator.SetFloat("VAx", movement.y);
        }
    }
}