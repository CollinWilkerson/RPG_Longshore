using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class HeaderInfo : MonoBehaviourPun
{
    public TextMeshProUGUI nameText;
    public Image bar;
    private float maxValue;


    public void Initialize(string text, float maxVal)
    {
        nameText.text = text;
        maxValue = maxVal;
        bar.fillAmount = 1f;
    }

    [PunRPC]
    public void UpdateHealthBar(float value)
    {
        //percentage of health as the fill amount
        bar.fillAmount = (float)value / maxValue;
    }
}
