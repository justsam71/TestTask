using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float currentHealth { get; }
    float maxHealth { get; }
    void TakeDamage(float value);
    void Die();
}
