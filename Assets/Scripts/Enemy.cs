using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public enum State
    {
        IDLE,
        CHASING,
        ATTACKING
    }
    private State currentState;

    private NavMeshAgent agent;
    private GameObject target;
    public GameObject deathEffect;

    private Entity targetEntity;

    private float attackDistance = 0.75f;
    private float attackRate = 1.0f;
    private float damage = 1.0f;

    private float nextAttackTime;

    private float enemyCollisionRadius;
    private float playerCollisionRadius;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();

        if (GameObject.FindGameObjectsWithTag("Player") != null )
        {
            currentState = State.CHASING;
            target = GameObject.FindGameObjectWithTag("Player");

            if (target != null)
            {
                targetEntity = target.GetComponent<Entity>();
                targetEntity.OnDeath += onPlayerDeath;
                enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
                playerCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
            }
        }
    }

    public override void TakeHit(float damage, Vector3 impactPoint, Vector3 impactDirection)
    {
        if (damage >= m_health)
        {
            Destroy(Instantiate(deathEffect, impactPoint, Quaternion.FromToRotation(Vector3.forward, impactDirection)), 2f); // Unity's good at mafs
        }
        base.TakeHit(damage, impactPoint, impactDirection);
    }

    private void onPlayerDeath()
    {
        if (target = null)
        {
            currentState = State.IDLE;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            if (Time.time > nextAttackTime)
            {
                float distToTarget = (target.transform.position - transform.position).sqrMagnitude;

                if (distToTarget < Mathf.Pow(attackDistance + enemyCollisionRadius + playerCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + attackRate;
                    StartCoroutine(Attack());
                }
            }
        }

            if (agent != null)
            {
                if (target != null)
                {
                    if (currentState == State.CHASING)
                    {
                        Vector3 targetDir = (target.transform.position - transform.position).normalized;
                        Vector3 targetPos = target.transform.position - targetDir * (enemyCollisionRadius + playerCollisionRadius + attackDistance / 2);

                        if (!isDead)
                        {
                            agent.SetDestination(targetPos);
                        }
                    }
                }
            }
    }

    private IEnumerator Attack()
    {
        currentState = State.ATTACKING;
        agent.enabled = false;

        Vector3 originalPos = transform.position;
        Vector3 targetDir = (target.transform.position - transform.position).normalized;
        Vector3 attackPos = target.transform.position  - targetDir * enemyCollisionRadius;

        float attackSpeed = 2.0f;
        float progress = 0;

        bool hasAttacked = false;

        while (progress <= 1)
        {
            if(progress >= 0.5f && !hasAttacked)
            {
                hasAttacked = true;
                targetEntity.TakeDamage(damage);
            }

            progress += Time.deltaTime * attackSpeed;
            float parabola = (Mathf.Pow(progress, 2) + progress) * 4; // Reference: https://www.mathsisfun.com/geometry/parabola.html
            transform.position = Vector3.Lerp(originalPos, attackPos, parabola);

            yield return null;
        }

        currentState = State.CHASING;
        agent.enabled = true;
    }

    public void SetAttributes(float enemySpeed, float enemyHealth, int hitsToKill)
    {
        agent.speed = enemySpeed;
        if (target != null)
        {
            damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKill);
        }
        startingHealth = enemyHealth;
    }
}
