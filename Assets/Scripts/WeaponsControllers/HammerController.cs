using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    [SerializeField]
    private AudioSource enter;
    [SerializeField]
    private AudioSource explosion;

    private int damage = 30;

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Attacking with Hammer");
            Debug.Log(col.gameObject.name + " touched");
            col.GetComponent<EnemyLife>().changeLife(damage);
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
