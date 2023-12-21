using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, EnemyLife
{

    public float speed;
    public float checkRadius;
    public float attackRadius;

    public bool shouldRotate;
    public LayerMask layerPlayer;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private Vector3 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;
    private bool isProtecting = false;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    Slider life;


    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isMoving", isInChaseRange);

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, layerPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, layerPlayer);

        if(shouldRotate)
        {
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
    }

    private void FixedUpdate(){
        dir = player.transform.position - transform.position;
        if(!isInAttackRange){
            anim.SetBool("isProtecting", false);
            isProtecting = false;
        }
        if(isInChaseRange && !isInAttackRange){
            rb.velocity = dir.normalized * speed;
        }
        if(isInAttackRange){
            rb.velocity = Vector2.zero;
            isProtecting = true;
            anim.SetBool("isProtecting", true);
        }
    }

    public void changeLife(int damage)
    {
        if(!isProtecting){
            life.value -= damage;
            if(life.value <= 0 ) 
            {
                Destroy(this.gameObject);
            }
        }
        
    }

  
}
