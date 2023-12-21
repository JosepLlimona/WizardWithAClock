using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    [SerializeField]
    private AudioSource enter;
    [SerializeField]
    private AudioSource explosion;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Attacking with Hammer");
            Debug.Log(col.gameObject.name + " touched");
            col.GetComponent<EnemyController>().changeLife(30);
        }
    }

    void PlayEnter()
    {
        enter.Play();
    }

    void PlayExplosion()
    {
        explosion.Play();
    }
}
