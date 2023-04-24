using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public Projectile bullet;
    public float fireRate = 100; // in ms
    public float bulletVelocity = 35;

    private float shotDelay;

    public void Shoot()
    {
        if (Time.time > shotDelay)
        {
            shotDelay = Time.time + fireRate / 1000;
            Projectile newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            newBullet.SetSpeed(bulletVelocity);
        }
    }
}
