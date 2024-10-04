using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public bool isWeaponMelee;
    public int weaponDamage;
    public float attackRadius;
    public float attackDelay, powerAttackDelay;
    public GameObject weaponPrefab;
    public AudioClip[] attackSFX;
    public AudioClip[] PowerAttackSFX;
}
