using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordThirdController : MonoBehaviour
{

    public GameObject player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && col != null)
        {
            Debug.Log("Attacking with Sword");
            col.GetComponent<EnemyLife>().changeLife(15);
        }
    }

    public void endAttack()
    {
        player.GetComponent<PlayerController>().endSTA();
        Destroy(this.gameObject);
    }
}
