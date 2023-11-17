using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsController : MonoBehaviour
{
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
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log(col.gameObject.name + " touched");
            col.GetComponent<EnemyController>().changeLife(1);
        }
    }
}
