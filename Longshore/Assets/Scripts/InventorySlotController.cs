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
    private InventoryController inventory;
    private Image image;
    public bool isFilled = false;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        inventory = FindFirstObjectByType<InventoryController>();
        SetInventorySlot(gameObject.GetComponent<WeaponData>());
    }

    public void OnItemSelect()
    {
        /*
        if(inventoryWeapon == null)
        {
            return;
        }
        */
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
