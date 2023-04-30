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
        equippedGun = Instantiate(gunToEquip, holdPoint.position, holdPoint.rotation);
        equippedGun.transform.parent = holdPoint;
    }

    public void Reload()
    {
        if (equippedGun == null)
        {
            equippedGun.Reload();
        }
    }

    public void OnTriggerHold()
    {
        if (equippedGun != null )
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null )
        {
            equippedGun.OnTriggerRelease();
        }
    }
}
