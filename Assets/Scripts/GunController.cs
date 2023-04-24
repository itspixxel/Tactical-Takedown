using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Gun equippedGun;
    public Gun starterGun;
    public Transform holdPoint;

    private void Start()
    {
        if (starterGun != null)
        {
            Equip(starterGun);
        }
    }
    
    public void Equip(Gun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate((Gun)gunToEquip, holdPoint.position, holdPoint.rotation);
        equippedGun.transform.parent = holdPoint;
    }

    public void Shoot()
    {
        if (equippedGun != null )
        {
            equippedGun.Shoot();
        }
    }
}
