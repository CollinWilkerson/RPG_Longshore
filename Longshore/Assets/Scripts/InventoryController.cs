using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InventoryController : MonoBehaviourPun
{
    private InventorySlotController[] slots;
    public PlayerController clientPlayer;

    private void Start()
    {
        slots = gameObject.GetComponentsInChildren<InventorySlotController>();
    }

    //make this an rpc
    [PunRPC]
    public void AddItem(WeaponData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(!slots[i].isFilled)
            {
                slots[i].SetInventorySlot(data);
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
