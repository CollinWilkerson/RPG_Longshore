using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this contols the buttons in the inventroy
/// should be placed on the sprite in the button
/// </summary>
public class InventorySlotController : MonoBehaviour
{
    
    private WeaponData inventoryWeapon;
    private Image image;
    public bool isFilled = false;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }

    public void OnItemSelect()
    {
        PlayerController player = FindFirstObjectByType<InventoryController>().clientPlayer;
        player.weapon.SetWeapon(inventoryWeapon);
    }

    //stores the data and changes the sprite
    public void SetInventorySlot(WeaponData newWeapon)
    {
        inventoryWeapon = newWeapon;
        image.color = Color.white;
        image.sprite = inventoryWeapon.weaponSprite;
        isFilled = true;
    }
}
