using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    public float startingHealth;

    protected float m_health;
    protected bool isDead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        m_health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 impactPoint, Vector3 impactDirection)
    {
        // Do some stuff here with hit var
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        m_health -= damage;

        if (m_health <= 0 && !isDead)
        {
            Die();
        }
    }

    protected void Die()
    {
        isDead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
