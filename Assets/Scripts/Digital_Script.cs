using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Digital_Script : MonoBehaviour, EnemyLife
{

    [SerializeField]
    private GameObject Num1;
    [SerializeField]
    private GameObject Num2;
    [SerializeField]
    private GameObject Num3;
    [SerializeField]
    private GameObject Num4;
    [SerializeField]
    private GameObject Num5;
    [SerializeField]
    private Transform numStart;
    [SerializeField]
    private AudioSource shot;
    private bool atack = false;
    private int num1 = 0;
    private int num2 = 0;
    private Vector2 objective;

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
    private bool isProtecting = false;
    private bool isAttacking = false;

    private int Number;

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

            if (Number == 1 && !isAttacking)
            {
                rb.velocity = Vector2.zero;
                isAttacking = true;
                anim.SetTrigger("Uno");
            }
            else if (Number == 2 && !isAttacking)
            {
                rb.velocity = Vector2.zero;
                isAttacking = true;
                anim.SetTrigger("Dos");
            }
            else if (Number == 3 && !isAttacking)
            {
                rb.velocity = Vector2.zero;
                isAttacking = true;
                anim.SetTrigger("Tres");
            }
            else if (Number == 4 && !isAttacking)
            {
                rb.velocity = Vector2.zero;
                isAttacking = true;
                anim.SetTrigger("Cuatro");
            }
            else if (Number == 5 && !isAttacking)
            {
                rb.velocity = Vector2.zero;
                isAttacking = true;
                anim.SetTrigger("Cinco");
            }

            if (!isInAttackRange)
            {
                isAttacking = false;
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
        life.value -= damage;
        if (life.value <= 0)
        {
            habitacio.GetComponent<GestioHabitacio>().nEnemics--;
            Destroy(this.gameObject);
        }

    }

    public void AcabarAtac()
    {
        isProtecting = false;
        Debug.Log("acabarP");
        Number = Random.Range(1, 6);
    }

    private IEnumerator nums()
    {

        while (true)
        {

            Number = Random.Range(1, 6);
            Debug.Log(Number);
            yield return new WaitForSeconds(6);

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

    public void Dispara1()
    {
        shot.Play();
        print("1 shot");
        Vector2 pos = numStart.transform.position;
        pos.x += 0.5f;
        GameObject NumInstance = Instantiate(Num1, pos, Quaternion.identity);
        NumInstance.GetComponent<Num1>().setPlayer(player);
        isAttacking = false;
    }

    public void Dispara2()
    {
        shot.Play();
        print("2 shot");
        Vector2 pos = numStart.transform.position;
        pos.x += 0.5f;
        GameObject NumInstance = Instantiate(Num2, pos, Quaternion.identity);
        NumInstance.GetComponent<Num2>().setPlayer(player);
        isAttacking = false;
    }

    public void Dispara3()
    {
        shot.Play();
        print("3 shot");
        Vector2 pos = numStart.transform.position;
        pos.x += 0.5f;
        GameObject NumInstance = Instantiate(Num3, pos, Quaternion.identity);
        NumInstance.GetComponent<Num3>().setPlayer(player);
        isAttacking = false;
    }

    public void Dispara4()
    {
        shot.Play();
        print("4 shot");
        Vector2 pos = numStart.transform.position;
        pos.x += 0.5f;
        GameObject NumInstance = Instantiate(Num4, pos, Quaternion.identity);
        NumInstance.GetComponent<Num4>().setPlayer(player);
        isAttacking = false;
    }

    public void Dispara5()
    {
        shot.Play();
        print("5 shot");
        Vector2 pos = numStart.transform.position;
        pos.x += 0.5f;
        GameObject NumInstance = Instantiate(Num5, pos, Quaternion.identity);
        NumInstance.GetComponent<Num5>().setPlayer(player);
        isAttacking = false;
    }

    void PlayShot()
    {
        shot.Play();
    }
}
