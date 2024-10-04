using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsMenu : MonoBehaviour
{
    public TextMeshProUGUI swordUsed, hammerUsed, bowUsed;
    private string usedWeaponName;

    void Awake()
    {
        usedWeaponName = WeaponManager.instance.usedWeapon.weaponName;
        if (usedWeaponName == "Sword")
        {
            swordUsed.enabled = true;
            hammerUsed.enabled = false;
            bowUsed.enabled = false;
        }
        else if (usedWeaponName == "Hammer")
        {
            swordUsed.enabled = false;
            hammerUsed.enabled = true;
            bowUsed.enabled = false;
        }
        else if (usedWeaponName == "Bow")
        {
            swordUsed.enabled = false;
            hammerUsed.enabled = false;
            bowUsed.enabled = true;
        }
    }

    public void ClickSFX()
    {
        AudioManager.instance.ClickSFX();
    }

    public void UseSword()
    {
        WeaponManager.instance.UseSword();
    }

    public void UseHammer()
    {
        WeaponManager.instance.UseHammer();
    }

    public void UseBow()
    {
        WeaponManager.instance.UseBow();
    }
}

