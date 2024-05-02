using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    [SerializeField] float _maxHealth = 100;

    public GameObject lifeCanvas;

    public float currentHealth;
    public bool canTakeDamage;

    public float damage = 25;

    private void Start()
    {
        currentHealth = _maxHealth;

        if (lifeCanvas != null)
        {
            Debug.Log("Canvas encontrado. Pertenece a: " + lifeCanvas.name);
        }
        else
        {
            Debug.LogWarning("El canvas no está asignado. Asegúrate de asignar el canvas en el inspector.");
        }       
    }

    private void Update()
    {
        UpdateHealthBar();     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= damage;
            Debug.Log("Vida restante" + " " + currentHealth);

        }
    }

    public void UpdateHealthBar()
    {
        if (canTakeDamage)
        {
            currentHealth = Mathf.Max(0, currentHealth);
            _healthBar.value = (float)currentHealth / _maxHealth;
        }
        else canTakeDamage = false;
    }


}
