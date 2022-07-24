using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerData data;


    public bool CanBuyWeapon(int price)
    {
        return data.coins >= price;
    }

    public void BuyWeapon(int price)
    {
        data.coins -= price;
    }
}
