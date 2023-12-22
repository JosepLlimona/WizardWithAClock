using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelPuño : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        Debug.Log("Fist changed");
        player.GetComponent<PlayerController>().changeWeaponDamage(1, "fists");
    }
}
