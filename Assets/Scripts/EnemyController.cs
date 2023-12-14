using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    public float speed;
    public float checkRadius;
    public float attackRadius;

    public bool shouldRotate;
    public LayerMask layerPlayer;
    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private Vector3 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;

    [SerializeField]
    Slider life;


    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        anim.SetBool("isMoving", isInChaseRange);

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, layerPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, layerPlayer);

        dir = target.position = transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;
        if(shouldRotate)
        {
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
    }

    private void FixedUpdate(){
        if(isInChaseRange && !isInAttackRange){
            MoveCharacter(movement);
        }
        if(isInAttackRange){
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveCharacter(Vector2 dir){
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }


/*    
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
 */
    public void changeLife(int damage)
    {
        life.value -= damage;
        if(life.value <= 0 ) 
        {
            Destroy(this.gameObject);
        }
    }

  
}
