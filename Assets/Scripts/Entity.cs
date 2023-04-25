using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    public float health;

    protected float m_health;
    protected bool isAlive = true;

    protected virtual void Start()
    {
        m_health = health;
    }

    public void TakeHit(float damageAmount, RaycastHit hit)
    {
        m_health -= damageAmount;

        if (m_health <= 0 && isAlive)
        {
            isAlive = false;
            Destroy(gameObject);
        }
    }
}
