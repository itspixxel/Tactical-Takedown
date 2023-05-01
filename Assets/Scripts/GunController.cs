using UnityEngine;

public class GunController : MonoBehaviour
{
    Gun equippedGun;

    public Gun[] guns;
    public Transform holdPoint;

    private void Start()
    {
        
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
    
    public void Equip(int gunIndex)
    {
        Equip(guns[gunIndex]);
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
