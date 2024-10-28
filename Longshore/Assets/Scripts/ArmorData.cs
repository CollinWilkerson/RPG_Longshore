using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public enum ArmorType
{
    helmet,
    chestplate,
    boots
}

public class ArmorData : MonoBehaviourPun
{
    [Header("General")]
    public ArmorType type;
    public Sprite armorSprite;
    public int defense = 0;
    public int goldValue;
    [Header("Helmet")]
    public SpriteRenderer HelmetSR;
    public int helmetDefense = 0;
    public float helmetDamgeBoost = 1;
    [Header("Chest")]
    public SpriteRenderer ChestSR;
    public int chestDefense = 0;
    public float healthRegen = 0;
    public float damageReflect = 0f;
    [Header("Boots")]
    public SpriteRenderer bootsSR;
    public int bootsDefense = 0;
    public float bootsSpeed = 1;
    
    [PunRPC]
    public void GetChestArmor(int index)
    {
        ArmorData data = ArmorCatalogue.catalogue[index];
        chestDefense = data.chestDefense;
        healthRegen = data.healthRegen;
        damageReflect = data.damageReflect;

        type = ArmorType.chestplate;

        defense = chestDefense + bootsDefense + helmetDefense;

        if (ChestSR == null)
        {
            return;
        }

        ChestSR.sprite = data.armorSprite;
    }

    [PunRPC]
    public void GetBoots(int index)
    {
        ArmorData data = ArmorCatalogue.catalogue[index];
        bootsDefense = data.bootsDefense;
        bootsSpeed = data.bootsSpeed;

        type = ArmorType.boots;

        defense = chestDefense + bootsDefense + helmetDefense;

        if (bootsSR == null)
        {
            return;
        }

        bootsSR.sprite = data.armorSprite;
    }

    [PunRPC]
    public void GetHelmet(int index)
    {
        ArmorData data = ArmorCatalogue.catalogue[index];
        helmetDefense = data.helmetDefense;
        helmetDamgeBoost = data.helmetDamgeBoost;

        type = ArmorType.helmet;

        defense = chestDefense + bootsDefense + helmetDefense;

        if (HelmetSR == null)
        {
            return;
        }

        HelmetSR.sprite = data.armorSprite;
    }
}
