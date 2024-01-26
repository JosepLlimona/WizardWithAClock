using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestioPortal : MonoBehaviour
{
    void Start()
    {
        if (transform.parent != null && transform.parent.CompareTag("Habitacio Boss"))
        {
            gameObject.SetActive(false);
        }
        else{
            gameObject.SetActive(true);
        }
    }


    public void MostarPortal(){
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            GestioHabitacio habitacio = transform.parent.GetComponent<GestioHabitacio>();

            if(habitacio != null){
                habitacio.PortalActivat();
            }
        }
    }

}
