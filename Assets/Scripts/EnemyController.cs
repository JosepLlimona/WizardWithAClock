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

    private bool canMove = true;

    private GameObject player;

    public GameObject habitacio;

    [SerializeField]
    Slider life;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        StartCoroutine(nums());

    }

    private void Update()
    {
        anim.SetBool("isMoving", isInChaseRange);


        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, layerPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, layerPlayer);

        if (shouldRotate)
        {
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            dir = player.transform.position - transform.position;

            if (Protect == 4 && !isAttacking && !isProtecting)
            {
                rb.velocity = Vector2.zero;
                isProtecting = true;
                anim.SetBool("isProtecting", true);
            }
            else if (Protect >= 0 && Protect <= 3)
            {
                anim.SetBool("isProtecting", false);
                isProtecting = false;

            }

            if (!isInAttackRange)
            {
                anim.SetBool("isAttacking", false);
                isAttacking = false;
            }
            if (isInChaseRange && !isInAttackRange && !isProtecting)
            {
                rb.velocity = dir.normalized * speed;
            }
            if (isInAttackRange && !isProtecting)
            {
                rb.velocity = Vector2.zero;
                isAttacking = true;
                anim.SetBool("isAttacking", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            Debug.Log("pega");
            player.GetComponent<PlayerController>().lostLife(15);
        }
    }


    public void changeLife(int damage)
    {
        if (!isProtecting)
        {
            life.value -= damage;
            if (life.value <= 0)
            {
                habitacio.GetComponent<GestioHabitacio>().nEnemics--;
                Destroy(this.gameObject);
            }
        }

    }

    public void stop()
    {
        StartCoroutine(stopM());
    }

    private IEnumerator stopM()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        canMove = true;
    }
    public void AcabarProtect()
    {
        isProtecting = false;
        Debug.Log("acabarP");
        Protect = Random.Range(0, 9);
    }

    private IEnumerator nums()
    {

        while (true)
        {

            if (!isProtecting)
            {
                Protect = Random.Range(0, 9);
                Debug.Log(Protect);
            }

            yield return new WaitForSeconds(2);

        }

    }

    public GameObject Habitacio
    {
        get
        {
            return habitacio;
        }
        set
        {
            habitacio = value;
        }
    }
}
