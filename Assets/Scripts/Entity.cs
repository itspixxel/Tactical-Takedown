using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    public float startingHealth;

    protected float health;
    protected bool isDead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        // Do some stuff here with hit var
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
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
