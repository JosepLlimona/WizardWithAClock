using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelEspada : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        player.GetComponent<PlayerController>().changeWeaponDamage(15, "sword");
    }
}
