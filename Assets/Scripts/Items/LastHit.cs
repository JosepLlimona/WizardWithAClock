using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastHit : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        player.GetComponent<PlayerController>().activeItem("LastHit");
        Destroy(gameObject);
    }
}
