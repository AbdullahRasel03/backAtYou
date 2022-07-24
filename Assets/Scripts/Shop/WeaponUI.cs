using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUI : MonoBehaviour
{
    [SerializeField] private WeaponsData weaponData;
    [SerializeField] private Image weaponImg;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private HorizontalLayoutGroup layOut;
    [SerializeField] private GameObject coinIconObj;
    [SerializeField] private Button buyOrEquipBtn;

    public static event Action<WeaponsData> OnWeaponEquipped;
    public static event Action<WeaponsData> OnWeaponBought;

    private InventoryController inventoryController;
    private PlayerInventory playerInventory;

    void Start()
    {
        inventoryController = InventoryController.GetInstance();
        playerInventory = inventoryController.GetPlayerInventory();
        weaponImg.sprite = weaponData.weaponImage;
        weaponName.SetText(weaponData.weaponName);
        CheckButtonState();
    }

    public void CheckButtonState()
    {
        if (weaponData.buttonState == WeaponButtonState.Buy)
        {
            if (!playerInventory.CanBuyWeapon(weaponData.weaponPrice))
            {
                buttonText.color = Color.red;
            }

            else
            {
                buttonText.color = Color.white;
            }
            buttonText.SetText(weaponData.weaponPrice.ToString());
            buyOrEquipBtn.interactable = true;
            coinIconObj.SetActive(true);
            layOut.padding.right = 180;
        }

        else if (weaponData.buttonState == WeaponButtonState.Equip)
        {
            buttonText.SetText("Equip");
            buyOrEquipBtn.interactable = true;
            coinIconObj.SetActive(false);
            layOut.padding.right = 100;
        }

        else if (weaponData.buttonState == WeaponButtonState.Using)
        {
            buttonText.SetText("Equipped");
            buyOrEquipBtn.interactable = false;
            coinIconObj.SetActive(false);
            layOut.padding.right = 100;
        }
    }

    internal void SetNewButtonState(WeaponButtonState state)
    {
        this.weaponData.buttonState = state;
    }

    public void BuyEquipButtonClickEvent()
    {
        if (weaponData.buttonState == WeaponButtonState.Equip)
        {
            OnWeaponEquipped?.Invoke(this.weaponData);
        }

        else if (weaponData.buttonState == WeaponButtonState.Buy)
        {
            if (playerInventory.CanBuyWeapon(weaponData.weaponPrice))
            {
                playerInventory.BuyWeapon(weaponData.weaponPrice);
                OnWeaponBought?.Invoke(this.weaponData);
            }
        }

    }

    public WeaponsData GetWeaponData()
    {
        return this.weaponData;
    }
}


