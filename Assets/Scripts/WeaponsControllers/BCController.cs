using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCController : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().Play("BrokenClock");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && collision != null)
        {
            collision.GetComponent<EnemyLife>().changeLife(50);
        }
    }

    public void endAnim()
    {
        gameObject.SetActive(false);
    }
}
