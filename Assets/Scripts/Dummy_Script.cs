using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy_Script : MonoBehaviour, EnemyLife
{

    [SerializeField]
    private AudioSource steps;
    [SerializeField]
    private AudioSource steps2;
    [SerializeField]
    private AudioSource steps3;
    [SerializeField]
    private AudioSource steps4;
    [SerializeField]
    private AudioSource attack;

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
    private bool isAttacking = false;

    private GameObject player;
    public GameObject habitacio;

    private bool canMove = true;

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
            if (!isInAttackRange)
            {
                anim.SetBool("isAttacking", false);
                isAttacking = false;
            }
            if (isInChaseRange && !isInAttackRange)
            {
                rb.velocity = dir.normalized * speed;
            }
            if (isInAttackRange)
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
            Debug.Log("danya");
            player.GetComponent<PlayerController>().lostLife(9);
        }
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

    void PlayStep()
    {
        steps.Play();
    }

    void PlayStep2()
    {
        steps2.Play();
    }

    void PlayStep3()
    {
        steps3.Play();
    }

    void PlayStep4()
    {
        steps4.Play();
    }

    void PlayAttack()
    {
        attack.Play();
    }
}
