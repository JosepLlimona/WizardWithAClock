using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvanceRapido : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        player.GetComponent<PlayerController>().changeSpeed(2);
        Destroy(this.gameObject);
    }
}
