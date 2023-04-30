using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum Mode
    {
        SINGLE,
        BURST,
        AUTO
    }
    public Mode mode;

    public Transform[] muzzles;
    public Transform bulletSpawnPoint;
    public Projectile bullet;
    public float fireRate = 100; // in ms
    public float bulletVelocity = 35;
    public float reloadTime;

    // Burst vars
    public int burstShotsCount;
    private int burstShotsRemaining;

    // Reload vars
    public int magSize;
    private int bulletsRemainingInMag;
    private bool isReloading;

    private float shotDelay;

    private bool isTriggerReleased;

    private void Start()
    {
        burstShotsRemaining = burstShotsCount;
        bulletsRemainingInMag = magSize;
    }

    private void Update()
    {
        if(!isReloading && bulletsRemainingInMag == 0)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (!isReloading && Time.time > shotDelay && bulletsRemainingInMag > 0)
        {
            if (mode == Mode.BURST)
            {
                if (burstShotsRemaining == 0) { return; }
                burstShotsRemaining--;
            }
            else if (mode == Mode.SINGLE)
            {
                if (!isTriggerReleased) { return; }
            }

            for (int i = 0; i < muzzles.Length; i++)
            {
                if (bulletsRemainingInMag == 0)
                {
                    break;
                }
                bulletsRemainingInMag--;
                shotDelay = Time.time + fireRate / 1000;
                Projectile newProjectile = Instantiate(bullet, muzzles[i].position, muzzles[i].rotation);
                newProjectile.SetSpeed(bulletVelocity);
            }
        }
    }

    public void Reload()
    {
        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        isReloading = true;
        yield return new WaitForSeconds(0.2f);

        float reloadSpeed = 1f / reloadTime;
        float reloadProgress = 0;

        while (reloadProgress < 1)
        {
            reloadProgress += Time.deltaTime * reloadSpeed;

            yield return null;
        }

        isReloading = false;
        bulletsRemainingInMag = magSize;
    }

    public void OnTriggerHold()
    {
        Shoot();
        isTriggerReleased = false;
    }

    public void OnTriggerRelease()
    {
        isTriggerReleased = true;
        burstShotsRemaining = burstShotsCount;
    }
}
