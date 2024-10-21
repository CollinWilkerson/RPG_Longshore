using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private bool inRange;
    public GameObject npcScreen;
    private InventoryController npcItems;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inRange = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        inRange = false;
    }

    private void Update()
    {
        if(inRange && Input.GetKeyDown(KeyCode.E) && !npcScreen.activeSelf)
        {
            npcScreen.SetActive(true);
        }
        else if (!inRange)
        {
            npcScreen.SetActive(false);
        }
        else if (npcScreen.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            npcScreen.SetActive(false);
        }
    }
}
