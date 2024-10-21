using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********************************************************************************
 * This script contains all of the data nesesary to give the player a weapon    *
 * It should be used for pickups and inventory                                  *
 ********************************************************************************/
public class WeaponData : MonoBehaviour
{
    [Header("Type")]
    public string weaponName;
    public string weaponType;
    public Sprite weaponSprite;

    [Header("Stats")]
    public int damage;
    public float attackRate;
    public float attackRange;
    public float attackSweep;
    public int goldValue;

    /*
    public void CopyData(WeaponData data)
    {
        weaponName = data.weaponName;
        weaponType = data.weaponType;
        weaponSprite = data.weaponSprite;
        damage = data.damage;
        attackRate = data.attackRate;
        attackRange = data.attackRange;
        attackSweep = data.attackSweep;
    }
    */
}
