using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private InventorySlotController[] slots;

    private void Start()
    {
        slots = FindObjectsByType<InventorySlotController>(0);
    }

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
}
