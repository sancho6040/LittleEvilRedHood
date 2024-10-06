using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int MaxHealth;
    public int _currentHealth;

    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public void TakeDamage(int inDamage)
    {
        _currentHealth -= inDamage;
        if (_currentHealth <= 0) Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
