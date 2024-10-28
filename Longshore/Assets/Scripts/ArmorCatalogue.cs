using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCatalogue : MonoBehaviour
{
    public static ArmorData[] catalogue;

    private void Awake()
    {
        catalogue = gameObject.GetComponents<ArmorData>();
    }
}
