using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsController : MonoBehaviour
{
    private bool hit = false;
    public int damage = 1;

    public void setDamage(int damage)
    {
        this.damage += damage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col == null)
        {
            Debug.Log("Coll null");
        }


        if (col.gameObject.tag == "Enemy")
        {
            hit = true;
            Debug.Log("Attacking with fists");
            col.GetComponent<EnemyLife>().changeLife(damage);
            GetComponentInParent<PlayerController>().playPunchAudio(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            hit = false;
        }
    }

    public void punch()
    {
        if (!hit)
        {
            GetComponentInParent<PlayerController>().playPunchAudio(false);
        }
    }
}
