using System.Net.Mime;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
public static class DataSaver
{
    public static void SaveWeaponUIButtonStates(List<WeaponUI> weapons)
    {
        BinaryFormatter formatter = new BinaryFormatter();

#if UNITY_EDITOR 
        string path = "/Users/pixelman360/Documents/Big Bang Projects/BackAtYou/Assets/Persistent Data/WeaponShopData.dat";

#else

        string path = Application.persistentDataPath + "/weaponShop.dat";
#endif

        FileStream stream = new FileStream(path, FileMode.Create);

        AvailableWeaponsList data = new AvailableWeaponsList(weapons);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static List<WeaponUI> LoadWeaponUIButtonStates()
    {
#if UNITY_EDITOR 
        string path = "/Users/pixelman360/Documents/Big Bang Projects/BackAtYou/Assets/Persistent Data/WeaponShopData.dat";

#else

        string path = Application.persistentDataPath + "/weaponShop.dat";
#endif

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AvailableWeaponsList data = formatter.Deserialize(stream) as AvailableWeaponsList;

            stream.Close();

            return data.GetWeaponsList();
        }

        Debug.LogError("No Saved Data Found!");

        return null;
    }
}