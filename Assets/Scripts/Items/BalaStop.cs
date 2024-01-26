using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaStop : MonoBehaviour, Items
{
    public void activeItem(GameObject player)
    {
        player.GetComponent<PlayerController>().activeItem("BalaStop");
        Destroy(gameObject);
    }
}
