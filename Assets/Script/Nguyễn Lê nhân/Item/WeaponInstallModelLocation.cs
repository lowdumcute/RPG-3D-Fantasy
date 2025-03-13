using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstallModelLocation : MonoBehaviour
{
    public WeaponModelSlot weaponSlot;
    public GameObject currentWeapon;

    public void UnloadWeapon()
    {
        if(currentWeapon !=null)
        {
            Destroy(currentWeapon);
        }
    }
    public void LoadWeapon(GameObject weaponModel)
    {
        currentWeapon = weaponModel;
        weaponModel.transform.parent = transform;
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }
}
