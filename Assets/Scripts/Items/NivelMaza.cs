using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelMaza : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        player.GetComponent<PlayerController>().changeWeaponDamage(30, "hammer");
    }
}
