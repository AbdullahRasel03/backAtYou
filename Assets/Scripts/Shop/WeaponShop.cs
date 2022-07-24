using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class WeaponShop : MonoBehaviour
{
    [SerializeField] private List<WeaponUI> weapons;
    [SerializeField] private RectTransform contentTransform;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private float scrollOffset;

    [Tooltip("Less is More Here.")]
    [SerializeField] private float scrollSpeed;

    private List<WeaponUI> listedWeapons = new List<WeaponUI>();
    private int nextIndex = 0;
    private float newVal;
    private bool canClick = true;

    void Awake()
    {
        // List<WeaponUI> allWeapons = DataSaver.LoadWeaponUIButtonStates();

        // if (allWeapons == null)
        // {
        //     foreach (WeaponUI weapon in weaponsData)
        //     {
        //         if (weapon.GetWeaponData().weaponName == "Katana")
        //         {
        //             weapon.GetWeaponData().buttonState = WeaponButtonState.Using;
        //         }
        //     }

        //     DataSaver.SaveWeaponUIButtonStates(weaponsData);
        // }

        // else
        // {
        //     for (int i = 0; i < allWeapons.Count; i++)
        //     {
        //         weaponsData[i].GetWeaponData().buttonState = allWeapons[i].GetWeaponData().buttonState;
        //     }
        // }

    }

    void Start()
    {
        nextIndex = 0;
        canClick = true;

        leftButton.interactable = false;
        rightButton.interactable = true;
    }

    void OnEnable()
    {
        WeaponUI.OnWeaponEquipped += SetCurrentEquippedWeapon;
        WeaponUI.OnWeaponBought += SetCurrentBoughtWeapon;

        InventoryController inventory = InventoryController.GetInstance();

        foreach (WeaponUI weapon in weapons)
        {
            WeaponsData data = weapon.GetWeaponData();

            int val = (data.weaponName == "Katana") ? PlayerPrefs.GetInt(data.weaponName, 2) : PlayerPrefs.GetInt(data.weaponName, 0);

            if (val == 0)
            {
                weapon.SetNewButtonState(WeaponButtonState.Buy);
            }

            else if (val == 1)
            {
                weapon.SetNewButtonState(WeaponButtonState.Equip);
            }

            else if (val == 2)
            {
                weapon.SetNewButtonState(WeaponButtonState.Using);
            }

            inventory.AddWeapon(weapon);
        }
    }

    void OnDisable()
    {
        WeaponUI.OnWeaponEquipped -= SetCurrentEquippedWeapon;
        WeaponUI.OnWeaponBought -= SetCurrentBoughtWeapon;
    }



    private void SetCurrentEquippedWeapon(WeaponsData obj)
    {
        foreach (WeaponUI weaponUI in listedWeapons)
        {
            WeaponsData data = weaponUI.GetWeaponData();

            if (data.buttonState == WeaponButtonState.Using && data.weaponName != obj.weaponName)
            {
                weaponUI.SetNewButtonState(WeaponButtonState.Equip);
                PlayerPrefs.SetInt(data.weaponName, 1);
                weaponUI.CheckButtonState();
            }

            else if (data.buttonState == WeaponButtonState.Equip && data.weaponName == obj.weaponName)
            {
                weaponUI.SetNewButtonState(WeaponButtonState.Using);
                PlayerPrefs.SetInt(data.weaponName, 2);
                weaponUI.CheckButtonState();
                InventoryController.GetInstance().GiveWeaponToPlayer(data.weaponGFX);
            }
        }

        // DataSaver.SaveWeaponUIButtonStates(weapons);
    }

    private void SetCurrentBoughtWeapon(WeaponsData obj)
    {
        foreach (WeaponUI weaponUI in listedWeapons)
        {
            WeaponsData data = weaponUI.GetWeaponData();

            if (data.buttonState == WeaponButtonState.Buy && data.weaponName == obj.weaponName)
            {
                weaponUI.SetNewButtonState(WeaponButtonState.Equip);
                PlayerPrefs.SetInt(data.weaponName, 1);
                weaponUI.CheckButtonState();
                break;
            }
        }

        // DataSaver.SaveWeaponUIButtonStates(weaponsData);
    }

    public void LeftClickEvent()
    {
        if (!canClick)
            return;

        canClick = false;

        nextIndex--;
        nextIndex = Mathf.Clamp(nextIndex, 0, weapons.Count - 1);

        ScrollLeft();
        ToggleButtonInteraction();

        StartCoroutine(ResetClicks());

    }

    public void RightClickEvent()
    {
        if (!canClick)
            return;

        canClick = false;

        nextIndex++;
        nextIndex = Mathf.Clamp(nextIndex, 0, weapons.Count - 1);

        ScrollRight();
        ToggleButtonInteraction();

        StartCoroutine(ResetClicks());

    }

    private IEnumerator ResetClicks()
    {
        yield return new WaitForSeconds(scrollSpeed);
        canClick = true;
    }

    private void ScrollLeft()
    {
        newVal = contentTransform.localPosition.x + scrollOffset;
        contentTransform.DOLocalMoveX(newVal, scrollSpeed);
    }

    private void ScrollRight()
    {
        newVal = contentTransform.localPosition.x - scrollOffset;
        contentTransform.DOLocalMoveX(newVal, scrollSpeed);
    }

    private void ToggleButtonInteraction()
    {
        if (nextIndex == 0)
        {
            leftButton.interactable = false;
        }

        else if (nextIndex == weapons.Count - 1)
        {
            rightButton.interactable = false;
        }

        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    public void ShopButtonClickEvent()
    {
        nextIndex = 0;
        leftButton.interactable = false;
        rightButton.interactable = true;
        PopupController.GetInstance().popupShopPanel.ShowView();
        StartCoroutine(StartWeaponListing());
    }

    private IEnumerator StartWeaponListing()
    {
        yield return new WaitForSeconds(0.01f);
        contentTransform.localPosition = new Vector3(0, 0, 0);
        ListAllWeapons();
    }

    public void ShopExitClickEvent()
    {
        RemoveListedWeapons();
        PopupController.GetInstance().popupShopPanel.HideView();
    }

    private void ListAllWeapons()
    {
        foreach (WeaponUI obj in weapons)
        {
            listedWeapons.Add(Instantiate(obj, contentTransform.position, Quaternion.identity, contentTransform));
        }
    }

    private void RemoveListedWeapons()
    {
        foreach (WeaponUI obj in listedWeapons)
        {
            Destroy(obj.gameObject);
        }

        listedWeapons.Clear();
    }
}


