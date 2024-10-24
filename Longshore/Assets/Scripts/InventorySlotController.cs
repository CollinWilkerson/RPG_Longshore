using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//enum for what type of item it is then switch that in the onitemselect


/// <summary>
/// this contols the buttons in the inventroy
/// should be placed on the sprite in the button
/// </summary>
public class InventorySlotController : MonoBehaviour
{
    private WeaponData inventoryWeapon;
    private ArmorData inventoryArmor;
    private InventoryController inventory;
    private Image image;
    public bool isFilled = false;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        inventory = FindFirstObjectByType<InventoryController>();
        SetInventorySlot(gameObject.GetComponent<WeaponData>());
        SetInventorySlot(gameObject.GetComponent<ArmorData>());
    }

    public void OnItemSelect()
    {
        if (inventoryWeapon != null)
        {
            if (!inventory.vendorInventory)
            {
                //photon is not a fan of sprites over the network
                //could fix with a sprite dictionary, no time
                inventory.clientPlayer.weapon.SetWeapon(inventoryWeapon);
                inventory.DataDisplay.UpdateIcon(inventoryWeapon);
            }
            else
            {
                inventory.vendorController.DisplayItem(inventoryWeapon);
            }
        }
        else if (inventoryArmor != null)
        {
            if (!inventory.vendorInventory)
            {
                if (inventoryArmor.type == ArmorType.boots)
                {
                    inventory.clientPlayer.armor.GetBoots(inventoryArmor);
                }
                if (inventoryArmor.type == ArmorType.chestplate)
                {
                    inventory.clientPlayer.armor.GetChestArmor(inventoryArmor);
                }
                if (inventoryArmor.type == ArmorType.helmet)
                {
                    inventory.clientPlayer.armor.GetHelmet(inventoryArmor);
                }
                inventory.DataDisplay.UpdateIcon(inventoryArmor);
            }
            else
            {
                inventory.vendorController.DisplayItem(inventoryArmor);
            }
        }
    }

    //stores the data and changes the sprite
    public void SetInventorySlot(WeaponData newWeapon)
    {
        if(newWeapon == null)
        {
            return;
        }
        inventoryWeapon = newWeapon;
        image.color = Color.white;
        image.sprite = inventoryWeapon.weaponSprite;
        isFilled = true;
    }

    public void SetInventorySlot(ArmorData newArmor)
    {
        if (newArmor == null)
        {
            return;
        }
        inventoryArmor = newArmor;
        image.color = Color.white;
        image.sprite = inventoryArmor.armorSprite;
        isFilled = true;
    }

    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventory.mouseOver.SetActive(true);
        inventory.mouseOver.GetComponent<WeaponDisplay>().UpdateIcon(inventoryWeapon);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventory.mouseOver.SetActive(false);
    }
    */
}
