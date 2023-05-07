using UnityEngine;

public interface IDamageable
{
    void TakeHit(float damage, Vector3 impactPoint, Vector3 impactDirection);
    void TakeDamage(float damage);
}
