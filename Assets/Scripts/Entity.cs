using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    public float health;

    protected float m_health;
    protected bool isAlive = true;

    public event System.Action onDeath;

    protected virtual void Start()
    {
        m_health = health;
    }

    public void TakeDamage(float damageAmount, RaycastHit hit)
    {
        m_health -= damageAmount;

        if (m_health <= 0 && isAlive)
        {
            isAlive = false;
            if (onDeath != null)
            {
                onDeath();
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        m_health -= damage;

        if (m_health <= 0 && isAlive)
        {
            isAlive = false;
            Destroy(gameObject);
        }
    }
}
