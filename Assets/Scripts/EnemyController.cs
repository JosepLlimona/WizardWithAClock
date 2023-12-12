using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    Slider life;
    private Transform playerPos;
    private Vector2 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeLife(int damage)
    {
        life.value -= damage;
        if(life.value <= 0 ) 
        {
            Destroy(this.gameObject);
        }
    }
}
