using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandBomb_Script : MonoBehaviour, EnemyLife
{

    [SerializeField]
    private AudioSource steps;
    [SerializeField]
    private AudioSource steps2;
    [SerializeField]
    private AudioSource explo;

    public float speed;
    public float checkRadius;
    public float attackRadius;
    private bool canMove = true;

    public bool shouldRotate;
    public LayerMask layerPlayer;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private Vector3 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;
    private bool hit = false;

    private GameObject player;
    public GameObject habitacio;

    [SerializeField]
    Slider life;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {


        anim.SetBool("isMoving", isInChaseRange);
        anim.SetBool("isExploding", isInAttackRange);

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
            if (isInChaseRange && !isInAttackRange)
            {
                rb.velocity = dir.normalized * speed;
            }
            if (isInAttackRange)
            {
                rb.velocity = Vector2.zero;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            Debug.Log("entro");
            hit = true;
            player = col.gameObject;
        }
    }


    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            Debug.Log("entro");
            hit = false;
        }
    }

    public void Explode()
    {

        if (hit)
        {
            player.GetComponent<PlayerController>().lostLife(11);
        }
        habitacio.GetComponent<GestioHabitacio>().nEnemics--;
        Destroy(this.gameObject);
    }

    public void changeLife(int damage)
    {
        life.value -= damage;
        if (life.value <= 0)
        {
            habitacio.GetComponent<GestioHabitacio>().nEnemics--;
            Destroy(this.gameObject);
        }

    }

    void PlayStep()
    {
        steps.Play();
    }

    void PlayStep2()
    {
        steps2.Play();
    }

    void PlayExplosion()
    {
        explo.Play();
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
