using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsController : MonoBehaviour
{
    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            col.GetComponent<EnemyLife>().changeLife(15);
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
