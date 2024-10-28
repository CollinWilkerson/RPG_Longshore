using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InventoryController : MonoBehaviourPun
{
    private InventorySlotController[] slots;
    public WeaponDisplay DataDisplay;
    public PlayerController clientPlayer;

    [Header("For Merchant")]
    public bool vendorInventory;
    public NPCController vendorController;

    private void Start()
    {
        DataDisplay = FindFirstObjectByType<WeaponDisplay>();
        slots = gameObject.GetComponentsInChildren<InventorySlotController>();
    }


    public void AddItem(int index, bool isArmor)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(!slots[i].isFilled)
            {
                if (isArmor)
                {
                    slots[i].SetInventorySlotArmor(index);
                }
                else
                {
                    slots[i].SetInventorySlot(index);
                }
                return;
            }
        }
        Debug.Log("Inventory Full");
    }

    public void SetClient(PlayerController client)
    {
        clientPlayer = client;
    }
}
