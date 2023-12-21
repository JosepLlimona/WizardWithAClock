using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SwordController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && col != null)
        {
            Debug.Log("Attacking with Sword");
            col.GetComponent<EnemyLife>().changeLife(15);
        }
    }

    private void AniamtionEnds(string name)
    {
        Debug.Log("Animation End: " + name);
        GetComponentInParent<PlayerController>().continueCombo(name);
    }
}
