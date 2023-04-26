using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask layerMask;

    private float speed = 10.0f;
    private float damage = 1.0f;

    float lifetime = 2.0f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float moveDistance = Time.deltaTime * speed;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            onHitActor(hit);
        }
    }

    void onHitActor(RaycastHit hit)
    {
        IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, hit);
        }

        GameObject.Destroy(gameObject);
    }
}