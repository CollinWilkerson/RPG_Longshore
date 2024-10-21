using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum NPCType
{
    Sell,
    Talk
}

public class NPCController : MonoBehaviour
{
    private bool inRange;
    public GameObject npcScreen;
    //public Sprite portrait;
    private InventoryController npcItems;
    public LayerMask playerMask;
    private PlayerController client;
    public NPCType type;

    private void Awake()
    {
        if (type == NPCType.Sell)
        {
            npcItems = npcScreen.GetComponentInChildren<InventoryController>();
        }
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
            //FindAnyObjectByType<PortraitController>().SetPortrait(portrait);
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
            if (type == NPCType.Sell)
            {
                npcItems.SetClient(client);
            }
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
