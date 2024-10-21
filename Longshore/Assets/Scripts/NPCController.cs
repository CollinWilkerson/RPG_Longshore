using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private bool inRange;
    public GameObject npcScreen;
    private InventoryController npcItems;
    public LayerMask playerMask;
    private PlayerController client;

    private void Awake()
    {
        npcItems = npcScreen.GetComponentInChildren<InventoryController>();
    }

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
            if (client == null)
            {
                Collider2D[] hitCheck = Physics2D.OverlapCircleAll(transform.position, 2f, playerMask);
                foreach (Collider2D collider in hitCheck)
                {
                    if (collider.gameObject.GetComponent<PlayerController>().IsClientPlayer())
                    {
                        client = collider.gameObject.GetComponent<PlayerController>();
                        Debug.Log("Client Set: " + client);
                    }
                }
            }
            npcItems.SetClient(client);
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
