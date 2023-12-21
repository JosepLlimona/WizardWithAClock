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
    private bool isAttacking = false;
    private int Protect;

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

        Protect = Random.Range(0, 11);

        dir = player.transform.position - transform.position;
        if(Protect == 4 && !isAttacking){
            rb.velocity = Vector2.zero;
            isProtecting = true;
            anim.SetBool("isProtecting", true);
        }
        else if(Protect >= 0 && Protect <= 3){
            anim.SetBool("isProtecting", false);
            isProtecting = false;
            
        }
        
        if(!isInAttackRange){
            anim.SetBool("isAttacking", false);
            isAttacking = false;
        }
        if(isInChaseRange && !isInAttackRange && !isProtecting){
            rb.velocity = dir.normalized * speed;
        }
        if(isInAttackRange && !isProtecting){
            rb.velocity = Vector2.zero;
            isAttacking = true;
            anim.SetBool("isAttacking", true);
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
