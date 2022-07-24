using System;
using System.Collections.Generic;

[Serializable]
public class AvailableWeaponsList
{
    public WeaponUI[] weapons;

    public AvailableWeaponsList(List<WeaponUI> weapons)
    {
        this.weapons = new WeaponUI[weapons.Count];

        for (int i = 0; i < weapons.Count; i++)
        {
            this.weapons[i] = weapons[i];
        }
    }

    public List<WeaponUI> GetWeaponsList()
    {
        List<WeaponUI> temp = new List<WeaponUI>();

        for (int i = 0; i < weapons.Length; i++)
        {
            temp.Add(weapons[i]);
        }
        return temp;
    }
}