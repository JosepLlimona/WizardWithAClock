using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchInstance : MonoBehaviour
{
    public void DestroyPunch()
    {
        print("destroying");
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Tocant desde puny");
            other.gameObject.GetComponent<PlayerController>().lostLife(5);
        }
    }
}
