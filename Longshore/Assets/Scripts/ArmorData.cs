using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ArmorType
{
    helmet,
    chestplate,
    boots
}

public class ArmorData : MonoBehaviour
{
    public ArmorType type;
    public Sprite armorSprite;
    public int defense = 0;
    public int goldValue;

    public int bootsDefense = 0;
    public float bootsSpeed = 1;

    public int helmetDefense = 0;
    public float helmetDamgeBoost = 1;

    public int chestDefense = 0;
    public float healthRegen = 0;
    public float damageReflect = 0f;

    public void GetChestArmor(ArmorData data)
    {
        chestDefense = data.chestDefense;
        healthRegen = data.healthRegen;
        damageReflect = data.damageReflect;

        type = ArmorType.chestplate;

        defense = chestDefense + bootsDefense + helmetDefense;
    }

    public void GetBoots(ArmorData data)
    {
        bootsDefense = data.bootsDefense;
        bootsSpeed = data.bootsSpeed;

        type = ArmorType.boots;

        defense = chestDefense + bootsDefense + helmetDefense;
    }
    public void GetHelmet(ArmorData data)
    {
        helmetDefense = data.helmetDefense;
        helmetDamgeBoost = data.helmetDamgeBoost;

        type = ArmorType.helmet;

        defense = chestDefense + bootsDefense + helmetDefense;
    }
}
