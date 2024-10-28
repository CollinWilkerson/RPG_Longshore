using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCatalogue : MonoBehaviour
{
    public static WeaponData[] catalogue;

    private void Awake()
    {
        catalogue = gameObject.GetComponents<WeaponData>();
    }
}
