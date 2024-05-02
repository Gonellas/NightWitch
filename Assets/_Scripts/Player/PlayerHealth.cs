
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    [SerializeField] float _maxHealth = 100;

    public float currentHealth;
    public bool canTakeDamage;

    public float damage = 25;

    private void Start()
    {
        currentHealth = _maxHealth;
    }

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
            _healthBar.value = currentHealth - damage;
        }
        else canTakeDamage = false;
    }
}
