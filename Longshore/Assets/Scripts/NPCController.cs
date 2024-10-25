using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum NPCType
{
    Merchant,
    Apothecary
}

public class NPCController : MonoBehaviour
{
    private bool inRange;

    [Header("NPC Inventory")]
    public GameObject npcScreen;
    private InventoryController npcItems;
    //top button
    public Button topButton;
    private TextMeshProUGUI topButtonText;
    //bottom button
    public Button bottomButton;
    private TextMeshProUGUI bottomButtonText;

    public TextMeshProUGUI textBox;
    private int interactionLevel;
    private bool interactionEnded;

    public LayerMask playerMask;
    private PlayerController client;
    public NPCType type;
    public GameObject toolTip;
    private WeaponData weaponToSell;
    private ArmorData armorToSell;
    private bool sellingWeapon;


    private void Awake()
    {
        if (type == NPCType.Merchant)
        {
            npcItems = npcScreen.GetComponentInChildren<InventoryController>();
        }

        topButtonText = topButton.GetComponentInChildren<TextMeshProUGUI>();
        bottomButtonText = bottomButton.GetComponentInChildren<TextMeshProUGUI>();
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
        //controlls the ability to interact with NPCs and pull up their interaction screen
        if (inRange)
        {
            toolTip.SetActive(true);
        }
        if (inRange && Input.GetKeyDown(KeyCode.E) && !npcScreen.activeSelf)
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

                    }
                }
            }
            if (type == NPCType.Merchant)
            {
                npcItems.SetClient(client);
            }

            interactionLevel = 1; //reset to the first interaction
            interactionEnded = false;
            topButton.interactable = true;
            bottomButton.interactable = true;
        }
        else if (!inRange)
        {
            toolTip.SetActive(false);
            npcScreen.SetActive(false);
        }
        else if (npcScreen.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            npcScreen.SetActive(false);
        }

        //dialouge progression - uses bianary tree structure to store text
        if (!interactionEnded)
        {
            if (type == NPCType.Apothecary)
            {
                if (interactionLevel == 1)//initial
                {
                    textBox.text = "Apothecary: All banged up?";

                    topButtonText.text = "Heal (" + (int)(client.maxHp - client.curHp) / 10 + " gold)";
                    bottomButtonText.text = "Who're you?";
                }
                else if (interactionLevel == 2)//top - heal
                {
                    if (client.gold >= (client.maxHp - client.curHp) / 10)
                    {
                        textBox.text = "Apothecary: All better, be careful out there";
                    }
                    else if (client.gold == 0)
                    {
                        textBox.text = "Apothecary: Seriously, You've got nothing? I can fix you up but " +
                            "I can't make you any better at this.";
                    }
                    else
                    {
                        textBox.text = "Apothecary: I'll take what you've got. Try to do better before you " +
                            "get hurt again.";
                    }
                    HealPlayer();

                    //end of dialoge path
                    EndDialouge();
                }
                else if(interactionLevel == 3)//bottom - Who're you?
                {
                    textBox.text = "Apothecary: I have the pleasure of being the only doctor in all of" +
                        "longshore. Not that I have many patients. most people who come here go" +
                        "crazy before long. We call them the Lost.";
                    EndDialouge();
                }
            }
        }
    }

    //for apothecary
    public void HealPlayer()
    {
        
        client.gold = Mathf.Clamp(client.gold - (int)(client.maxHp - client.curHp) / 10, 0, client.gold);
        GameUI.instance.UpdateGoldText(client.gold);
        client.photonView.RPC("Heal", RpcTarget.All, client.maxHp);
    }

    //for merchant
    public void DisplayItem(WeaponData data)
    {
        weaponToSell = data;
        textBox.text = "Merchant: Ahh, you've got your eyes on " + data.weaponName +
            " the " + data.weaponType + ". ";
        if (data.weaponType == "Axe")
        {
            textBox.text = textBox.text + "It packs a real punch, but it's a bit slow.";
        }
        if (data.weaponType == "Sword")
        {
            textBox.text = textBox.text + "It's quick, but doesn't hit as hard.";
        }

        textBox.text = textBox.text+
            "\nDamage: " + data.damage +
            "\nRate: " + data.attackRate +
            "\nRange: " + data.attackRange;

        topButtonText.text = "Buy (" + data.goldValue + " gold)";
        bottomButton.interactable = false;
        sellingWeapon = true;
    }

    //for merchant
    public void DisplayItem(ArmorData data)
    {
        armorToSell = data;
        textBox.text = "Merchant: Ahh, you've got your eyes on a ";
        if (data.type == ArmorType.boots)
        {
            textBox.text = textBox.text + "pair of boots" +
                "\nDefense: " + data.bootsDefense +
                "\nSpeed: " + data.bootsSpeed;
        }
        if (data.type == ArmorType.chestplate)
        {
            textBox.text = textBox.text + "chestplate" + 
                "\nDefense: " + data.chestDefense +
                "\nRegen: " + data.healthRegen +
                "\nReflect: " + data.damageReflect;
        }
        if (data.type == ArmorType.helmet)
        {
            textBox.text = textBox.text + "helmet." +
                "\nDefense: " + data.helmetDefense +
                "\nDamage Multiplier: " + data.helmetDamgeBoost;
        }

        topButtonText.text = "Buy (" + data.goldValue + " gold)";
        bottomButton.interactable = false;
        sellingWeapon = false;
    }

    //for merchant
    public void SellItem()
    {
        if (sellingWeapon)
        {
            if (client.gold >= weaponToSell.goldValue)
            {
                client.inventory.GetComponent<InventoryController>().AddItem(weaponToSell);
                client.gold -= weaponToSell.goldValue;
                GameUI.instance.UpdateGoldText(client.gold);
                textBox.text = "Merchant: Thanks, mate.";
            }
            else
            {
                textBox.text = "Merchant: Come back when your a little richer, mate.";
            }
        }
        else
        {
            if (client.gold >= armorToSell.goldValue)
            {
                client.inventory.GetComponent<InventoryController>().AddItem(armorToSell);
                client.gold -= armorToSell.goldValue;
                GameUI.instance.UpdateGoldText(client.gold);
                textBox.text = "Merchant: Thanks, mate.";
            }
            else
            {
                textBox.text = "Merchant: Come back when your a little richer, mate.";
            }
        }
    }

    public void TopOption()
    {
        interactionLevel *= 2; //advances the dialoge
    }
    public void BottomOption()
    {
        interactionLevel *= 2;
        interactionLevel++; //advances the dialoge
    }
    private void EndDialouge()
    {
        topButton.interactable = false;
        topButtonText.text = "";
        bottomButton.interactable = false;
        bottomButtonText.text = "";
        interactionEnded = true;
    }
}
