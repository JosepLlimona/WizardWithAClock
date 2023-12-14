using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    Slider life;
    public Transform player;
    public float moveSpeed = 0.1f;
    private Vector2 movement;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate(){
        move(movement);
    }
    void move(Vector2 direction){
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        Debug.Log("Current Speed: " + movement);
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
