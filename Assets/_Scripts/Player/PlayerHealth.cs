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

    private void Start()
    {
        currentHealth = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    public void UpdateHealthBar()
    {
        if (canTakeDamage)
        {
            currentHealth = Mathf.Max(0, currentHealth);
            _healthBar.value = ((float)currentHealth / _maxHealth) * 100f;
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

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        GameManager.instance.Lose();
       
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.5f); 

        _animator.SetBool("isDamaged", false);
    }

}
