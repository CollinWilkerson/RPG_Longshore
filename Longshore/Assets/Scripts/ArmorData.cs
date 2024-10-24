using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorData : MonoBehaviour
{
    public int defense = 0;
    public float healthRegen = 0;
    public float damageReflect = 0f;

    public int bootsDefense = 0;
    public float bootsSpeed = 1;

    public int helmetDefense = 0;
    public float helmetDamgeBoost = 1;

    public int chestDefense = 0;

    public void GetChestArmor(ArmorData data)
    {
        chestDefense = data.defense;
        healthRegen = data.healthRegen;
        damageReflect = data.damageReflect;

        defense = chestDefense + bootsDefense + helmetDefense;
    }

    public void GetBoots(ArmorData data)
    {
        bootsDefense = data.bootsDefense;
        bootsSpeed = data.bootsSpeed;

        defense = chestDefense + bootsDefense + helmetDefense;
    }
    public void GetHelmet(ArmorData data)
    {
        helmetDefense = data.helmetDefense;
        helmetDamgeBoost = data.helmetDamgeBoost;

        defense = chestDefense + bootsDefense + helmetDefense;
    }
}
