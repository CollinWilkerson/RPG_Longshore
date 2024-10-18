using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this contols the buttons in the inventroy
/// should be placed on the sprite in the button
/// </summary>
public class InventorySlotController : MonoBehaviour
{
    //I need a way to dynamically set the player for this client
    public PlayerController player;
    private WeaponData inventoryWeapon;
    private SpriteRenderer sr;
    public bool isFilled = false;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnItemSelect()
    {
        player.weapon.SetWeapon(inventoryWeapon);
    }

    //stores the data and changes the sprite
    public void SetInventorySlot(WeaponData newWeapon)
    {
        inventoryWeapon = newWeapon;
        sr.color = Color.white;
        sr.sprite = inventoryWeapon.weaponSprite;
        isFilled = true;
    }
}
