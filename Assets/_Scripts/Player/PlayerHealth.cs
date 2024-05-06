using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Slider _healthBar;
    [SerializeField] Animator _animator;

    [Header("Values")]
    [SerializeField] float _maxHealth = 100;
    public float currentHealth;
    public bool canTakeDamage;

    //TEST DAÑO
    public float damage = 25;

    private void Start()
    {
        currentHealth = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    //TEST DAÑO
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            canTakeDamage = true;
            currentHealth -= damage;
            Debug.Log("Vida restante" + " " + currentHealth);
            UpdateHealthBar();
        }
    }

    public void UpdateHealthBar()
    {
        if (canTakeDamage)
        {
            _healthBar.value -= damage;
        }
        else canTakeDamage = false;
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
            UpdateHealthBar();
            Debug.Log("Vida restante: " + currentHealth);

            _animator.SetBool("isDamagedBool", true);
            _animator.SetTrigger("isDamaged");
          
            StartCoroutine(ResetAnimation());
        }
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.5f); 

        _animator.SetBool("isDamaged", false);
    }

}
