using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("HealthParameters")]
    [SerializeField] private float _maxHealth = 100; 
    private float _currentHealth;   
    public float currentHealth => _currentHealth;
    public float maxHealth => _maxHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
        EventBus.Publish(new HealthChangedEvent(this, _currentHealth, _maxHealth));
    }

    public void TakeDamage(float damageTaken)
    {
        _currentHealth = Mathf.Clamp(currentHealth - damageTaken, 0, maxHealth);

        EventBus.Publish(new HealthChangedEvent(this, _currentHealth, _maxHealth));

        if (_currentHealth == 0)
            Die();
    }

    public void Die()
    {
        EventBus.Publish(new DiedEvent(this));
    }
}
