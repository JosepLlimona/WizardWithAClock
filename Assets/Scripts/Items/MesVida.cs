using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesVida : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        player.GetComponent<PlayerController>().moreLife(25);
        Destroy(this.gameObject);
    }
}
