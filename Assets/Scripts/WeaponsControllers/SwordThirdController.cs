using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordThirdController : MonoBehaviour
{

    private GameObject player;
    private int damage = 15;

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }

    public void setDamage(int damage)
    {
        this.damage += damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && col != null)
        {
            Debug.Log("Attacking with Sword");
            col.GetComponent<EnemyLife>().changeLife(damage);
        }
    }

    public void endAttack()
    {
        player.GetComponent<PlayerController>().endSTA();
        Destroy(this.gameObject);
    }
}
