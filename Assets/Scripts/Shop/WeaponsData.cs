using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data")]
public class WeaponsData : ScriptableObject
{
    public string weaponName;
    public int weaponPrice;
    public GameObject weaponGFX;
    public Sprite weaponImage;
    public WeaponButtonState buttonState;
}

public enum WeaponButtonState
{
    Buy,
    Equip,
    Using
}
