using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//enum for what type of item it is then switch that in the onitemselect


/// <summary>
/// this contols the buttons in the inventroy
/// should be placed on the sprite in the button
/// </summary>
public class InventorySlotController : MonoBehaviour
{
    [SerializeField]
    private int inventoryWeapon;
    [SerializeField]
    private int inventoryArmor;
    private InventoryController inventory;
    private Image image;
    public bool isFilled = false;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        inventory = FindFirstObjectByType<InventoryController>();
        SetInventorySlot(inventoryWeapon);
        SetInventorySlotArmor(inventoryArmor);
    }

    public void OnItemSelect()
    {
        if (inventoryWeapon != -1)
        {
            if (!inventory.vendorInventory)
            {
                inventory.clientPlayer.weapon.photonView.RPC("SetWeapon", RpcTarget.All, inventoryWeapon);
                inventory.DataDisplay.UpdateIcon(inventoryWeapon);
            }
            else
            {
                inventory.vendorController.DisplayItem(inventoryWeapon);
            }
        }
        else if (inventoryArmor != -1)
        {
            if (!inventory.vendorInventory)
            {
                if (ArmorCatalogue.catalogue[inventoryArmor].type == ArmorType.boots)
                {
                    inventory.clientPlayer.armor.photonView.RPC("GetBoots",RpcTarget.All,inventoryArmor);
                }
                if (ArmorCatalogue.catalogue[inventoryArmor].type == ArmorType.chestplate)
                {
                    inventory.clientPlayer.armor.photonView.RPC("GetChestArmor", RpcTarget.All, inventoryArmor);
                }
                if (ArmorCatalogue.catalogue[inventoryArmor].type == ArmorType.helmet)
                {
                    inventory.clientPlayer.armor.photonView.RPC("GetHelmet", RpcTarget.All, inventoryArmor);
                }
                inventory.DataDisplay.UpdateIconArmor(inventoryArmor);
            }
            else
            {
                inventory.vendorController.DisplayArmor(inventoryArmor);
            }
        }
    }

    //stores the data and changes the sprite
    public void SetInventorySlot(int index)
    {
        if (index == -1 || index > WeaponCatalogue.catalogue.Length)
        {
            return;
        }
        Debug.Log("Collected: " + index + "\nName: " + WeaponCatalogue.catalogue[index].weaponName);
        inventoryWeapon = index;
        image.color = Color.white;
        image.sprite = WeaponCatalogue.catalogue[index].weaponSprite;
        isFilled = true;
    }

    public void SetInventorySlotArmor(int index)
    {
        if (index == -1 || index > ArmorCatalogue.catalogue.Length)
        {
            return;
        }
        inventoryArmor = index;
        image.color = Color.white;
        image.sprite = ArmorCatalogue.catalogue[index].armorSprite;
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
