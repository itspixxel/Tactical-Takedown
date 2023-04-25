using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    private NavMeshAgent agent;
    private GameObject target;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (agent != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }
}
