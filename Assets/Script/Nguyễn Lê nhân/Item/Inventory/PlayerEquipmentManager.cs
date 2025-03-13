using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    public PlayerManager playerManager;
    public WeaponInstallModelLocation currentRightHandSlot;
    public WeaponInstallModelLocation currentLeftHandSlot;
    public GameObject currentLeftHandGameObject;
    public GameObject currentRightHandGameObject;
    public WeaponInstallModelLocation[] weaponSlot;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        InstanceWeaponSlot();
        
    }
    protected override void Start()
    {
        base.Start();

        Invoke(nameof(LoadWeaponBothHand), 2f);

    }
    
    public void InstanceWeaponSlot()
    {
        weaponSlot = GetComponentsInChildren<WeaponInstallModelLocation>();
        foreach(var weaponslot in weaponSlot)
        {
            if(weaponslot.weaponSlot == WeaponModelSlot.Righthand)
            {
                currentRightHandSlot = weaponslot;
            }
            else if(weaponslot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                currentLeftHandSlot = weaponslot;
            }
        }
    }
    public void LoadWeaponBothHand()
    {
        LoadWeaponRightHand();
        LoadWeaponLeftHand();
    }
    public void LoadWeaponRightHand()
    {
        if (playerManager == null)
        {
            Debug.LogError("playerManager is NULL in PlayerEquipmentManager.");
            return;
        }
        if (playerManager.PlayerinventoryManager.currentRightHandWeapon != null)
        {

            currentRightHandGameObject = Instantiate(playerManager.PlayerinventoryManager.currentRightHandWeapon.WeaponPrefab);
            currentRightHandSlot.LoadWeapon(currentRightHandGameObject);
        }
    }
    public void LoadWeaponLeftHand()
    {
        if (playerManager.PlayerinventoryManager.currentLeftHandWeapon != null)
        {
            currentLeftHandGameObject = Instantiate(playerManager.PlayerinventoryManager.currentLeftHandWeapon.WeaponPrefab);
            currentLeftHandSlot.LoadWeapon(currentLeftHandGameObject);
        }
    }
}
