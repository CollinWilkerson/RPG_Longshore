using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// this contols the buttons in the inventroy
/// should be placed on the sprite in the button
/// </summary>
public class InventorySlotController : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    
    private WeaponData inventoryWeapon;
    private InventoryController inventory;
    private Image image;
    public bool isFilled = false;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        inventory = FindFirstObjectByType<InventoryController>();
    }

    public void OnItemSelect()
    {
        /*
        if(inventoryWeapon == null)
        {
            return;
        }
        */
        PlayerController player = inventory.clientPlayer;
        player.weapon.SetWeapon(inventoryWeapon);
        inventory.DataDisplay.UpdateIcon(inventoryWeapon);
    }

    //stores the data and changes the sprite
    public void SetInventorySlot(WeaponData newWeapon)
    {
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
