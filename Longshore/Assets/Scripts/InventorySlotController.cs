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
    private Image sr;
    public bool isFilled = false;

    private void Awake()
    {
        sr = gameObject.GetComponent<Image>();
    }

    public void OnItemSelect()
    {
        PlayerController player = FindFirstObjectByType<InventoryController>().clientPlayer;
        player.weapon.photonView.RPC("SetWeapon", player.photonPlayer, inventoryWeapon); //this doesnt need to be an rpc
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
