using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    [SerializeField] private TextMeshProUGUI coinAmount;


    void Start()
    {
        ChangeCoinText(data.coins.ToString());
    }

    public bool CanBuyWeapon(int price)
    {
        return data.coins >= price;
    }

    public void BuyWeapon(int price)
    {
        data.coins -= price;
        coinAmount.SetText(data.coins.ToString());
    }

    void ChangeCoinText(string text)
    {
        coinAmount.SetText(text);
    }
}
