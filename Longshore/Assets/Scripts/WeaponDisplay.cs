using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponDisplay : MonoBehaviour
{
    /*
    float width;
    float height;

    private void Start()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
    }
    private void Update()
    {
        float mouseX = Input.mousePosition.x + width/2;
        float mouseY = Input.mousePosition.y + height/2;

        transform.position = new Vector2(mouseX, mouseY);
    }
    */
    
    public void UpdateIcon(WeaponData data)
    {
        TextMeshProUGUI displayText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        displayText.text = "Name: " + data.weaponName + 
            "\nType: " + data.weaponType + 
            "\nDamage: " + data.damage + 
            "\nRate: " + data.attackRate + 
            "\nRange: " + data.attackRange;
    }
    public void UpdateIcon(ArmorData data)
    {

        TextMeshProUGUI displayText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (data.type == ArmorType.boots) {
            displayText.text = "Defense: " + data.bootsDefense +
                "\nSpeed: " + data.bootsSpeed;
        }
        if (data.type == ArmorType.chestplate)
        {
            displayText.text = "Defense: " + data.chestDefense +
                "\nRegen: " + data.healthRegen +
                "\nReflect: " + data.damageReflect;
        }
        if (data.type == ArmorType.helmet)
        {
            displayText.text = "Defense: " + data.helmetDefense +
                "\nDamage Multiplier: " + data.helmetDamgeBoost;
        }
    }
}
