using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class explosion : MonoBehaviour
{
    [SerializeField]
    private AudioSource explosionSound;
    public void DestroyExplosion()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Tocant desde explosió");
            other.gameObject.GetComponent<PlayerController>().lostLife(15);
        }
    }
    public void activate_explosion()
    {
        explosionSound.Play();
    }
}
