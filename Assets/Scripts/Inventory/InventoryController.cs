using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;

    [SerializeField] private PlayerInventory playerInventory;

    private List<WeaponUI> availableWeapons = new List<WeaponUI>();

    public static event Action OnWeaponIntantiated;

    void Awake()
    {
        if (InventoryController.instance != null)
        {
            Destroy(this);
        }
        InventoryController.instance = this;
    }

    void Start()
    {
        CheckCurrentPlayerWeapon();
    }

    public static InventoryController GetInstance()
    {
        return InventoryController.instance;
    }

    public void AddWeapon(WeaponUI weapon)
    {
        availableWeapons.Add(weapon);
    }

    public void RemoveAllWeapons()
    {
        availableWeapons.Clear();
    }

    private void CheckCurrentPlayerWeapon()
    {
        foreach (WeaponUI weapon in availableWeapons)
        {
            if (weapon.GetWeaponData().buttonState == WeaponButtonState.Using)
            {
                GiveWeaponToPlayer(weapon.GetWeaponData().weaponGFX);
                break;
            }
        }

    }

    public PlayerInventory GetPlayerInventory()
    {
        return this.playerInventory;
    }

    public void GiveWeaponToPlayer(GameObject obj)
    {
        Player player = LevelController.GetInstance().GetPlayer();
        Transform weaponTransform = player.GetComponentInChildren<Blade>().transform;
        GameObject oldWeapon = weaponTransform.GetChild(0).gameObject;

        if (oldWeapon != null)
            Destroy(oldWeapon);


        Instantiate(obj, weaponTransform.localPosition - new Vector3(0, 1.16f, -0.6f),
                Quaternion.Euler(180 - 20, 0, 0), weaponTransform);

        OnWeaponIntantiated?.Invoke();
    }

    void OnDestroy()
    {
        RemoveAllWeapons();
    }


}
