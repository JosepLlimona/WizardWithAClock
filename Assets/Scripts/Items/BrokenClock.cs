using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenClock : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        player.GetComponent<PlayerController>().activeItem("BrokenClock");
        Destroy(gameObject);
    }
}
