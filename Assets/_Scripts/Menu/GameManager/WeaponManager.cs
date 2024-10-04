using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public WeaponSO[] weaponsList;
    public WeaponSO usedWeapon;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        usedWeapon = weaponsList[PlayerPrefs.GetInt("weaponsIndex", 0)];
    }

    public void UseSword()
    {
        usedWeapon = weaponsList[0];
        PlayerPrefs.SetInt("weaponsIndex", 0);
    }

    public void UseHammer()
    {
        usedWeapon = weaponsList[1];
        PlayerPrefs.SetInt("weaponsIndex", 1);
    }

    public void UseBow()
    {
        usedWeapon = weaponsList[2];
        PlayerPrefs.SetInt("weaponsIndex", 2);

    }
}
