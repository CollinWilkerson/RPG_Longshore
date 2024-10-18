using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InventoryController : MonoBehaviour
{
    private InventorySlotController[] slots;
    public PlayerController clientPlayer;

    private void Start()
    {
        slots = FindObjectsByType<InventorySlotController>(0);
    }

    //make this an rpc
    public void AddItem(WeaponData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(!slots[i].isFilled)
            {
                slots[i].SetInventorySlot(data);
                break;
            }
        }
        Debug.Log("Inventory Full");
    }

    public void SetClient(PlayerController client)
    {
        clientPlayer = client;
    }
}
