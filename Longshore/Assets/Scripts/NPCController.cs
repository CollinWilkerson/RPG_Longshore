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
    Apothecary,
    Guide
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
    private bool interactionEnded = true;

    public LayerMask playerMask;
    private PlayerController client;
    public NPCType type;
    public GameObject toolTip;
    private int weaponToSell;
    private int armorToSell;
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
                else if (interactionLevel == 3)//bottom - Who're you?
                {
                    textBox.text = "Apothecary: I have the pleasure of being the only doctor in all of " +
                        "longshore. Not that I have many patients. most people who come here go " +
                        "crazy before long. We call them the Lost.";
                    EndDialouge();
                }
            }
            if (type == NPCType.Guide)
            {
                if (interactionLevel == 1)//initial
                {
                    textBox.text = "Guide: Welcome to Longshore!";
                    topButtonText.text = "Where am I?";
                    bottomButtonText.text = "What do I do?";
                }
                if(interactionLevel == 2)//Top - where am I
                {
                    textBox.text = "Guide: I wish I was sure myself, all we know is that this " +
                        "island is a frequent stop for crashing ships. brings newcomers like " +
                        "yourself.";
                    topButtonText.text = "We?";
                    bottomButtonText.text = "Your ship?";
                }
                if(interactionLevel == 3)//Bottom - what do i do
                {
                    textBox.text = "Guide: That's really up to you. You can press I to see what " +
                        "you've got. The people in town might be able to help you out if you're " +
                        "short on supplies.";
                    topButtonText.text = "Town?";
                    bottomButtonText.text = "Around Here?";
                }
                if(interactionLevel == 4)// TT - we?
                {
                    textBox.text = "Guide: Me and some other survivors, the ones that haven't " +
                        "gone crazy at least. There's a merchant, he might look shady but his goods " +
                        "are quality. He's always got old stock to get rid of if you need a weapon.";
                    EndDialouge();
                }
                if(interactionLevel == 5)//TB - Your Ship
                {
                    textBox.text = "Guide: The crown jewel of the Royal Navy of King Zedron, though " +
                        "I'm afraid he's lost it like all the others. He enlisted the help of a Devil " +
                        "up north to make sure that no one can leave.";
                    EndDialouge();
                }
                if(interactionLevel == 6)//BT - Town
                {
                    textBox.text = "Guide: A little settlement down this path, the folks there are " +
                        "friendly, might even help you get settled here.";
                    EndDialouge();
                }
                if(interactionLevel == 7)//BB - Around Here
                {
                    textBox.text = "Guide: To the North is an inferno of horrors and to the south is a" +
                        "swarm of angry crabs. Oh, and if you follow the path past the town you reach the" +
                        "castle where the insane king lives. Lovely place, isn't it?";
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
    //old data to data version
    /*
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
    */
    public void DisplayItem(int index)
    {
        WeaponData data = WeaponCatalogue.catalogue[index];
        weaponToSell = index;
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

        textBox.text = textBox.text +
            "\nDamage: " + data.damage +
            "\nRate: " + data.attackRate +
            "\nRange: " + data.attackRange;

        topButtonText.text = "Buy (" + data.goldValue + " gold)";
        bottomButton.interactable = false;
        sellingWeapon = true;
    }

    //for merchant
    public void DisplayArmor(int index)
    {
        ArmorData data = ArmorCatalogue.catalogue[index];
        armorToSell = index;
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
            WeaponData weaponToSellData = WeaponCatalogue.catalogue[weaponToSell];
            if (client.gold >= weaponToSellData.goldValue)
            {
                client.inventory.GetComponent<InventoryController>().AddItem(weaponToSell, false);
                client.gold -= weaponToSellData.goldValue;
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
            ArmorData armorToSellData = ArmorCatalogue.catalogue[armorToSell];
            if (client.gold >= armorToSellData.goldValue)
            {
                client.inventory.GetComponent<InventoryController>().AddItem(armorToSell, true);
                client.gold -= armorToSellData.goldValue;
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
