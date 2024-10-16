using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PickupType
{
    Gold,
    Health,
    Weapon
}

public class Pickup : MonoBehaviourPun
{
    public PickupType type;
    public int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(type == PickupType.Gold)
            {
                player.photonView.RPC("GiveGold", player.photonPlayer, value);
            }
            else if (type == PickupType.Health)
            {
                player.photonView.RPC("Heal", player.photonPlayer, value);
            }
            else if(type == PickupType.Weapon)
            {
                WeaponData data = gameObject.GetComponent<WeaponData>();
                WeaponController playerWeapon = collision.gameObject.GetComponent<WeaponController>();
                playerWeapon.photonView.RPC("SetWeapon", player.photonPlayer, data.weaponType, data.damage, data.attackRange, data.attackRate, data.attackSweep);
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
