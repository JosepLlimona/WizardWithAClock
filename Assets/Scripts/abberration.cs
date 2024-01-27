using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class abberration : MonoBehaviour, EnemyLife
{
    private float Direction = 4;
    private Rigidbody2D rbody;
    private Rigidbody2D Prbody;
    private GameObject player;
    [SerializeField]
    private int speed = 3;
    private float timemove = 1f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Animator BossAnim;
    private int LIFE = 250;
    private bool attacking = false;
    private int num = 0;
    private bool absorving = false;
    private int cooldown = 0;

    [SerializeField]
    private AudioSource vacum;

    public GameObject habitacio;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        InvokeRepeating("moveboss", 0f, timemove);
        player = GameObject.Find("Player"); 
        Prbody= player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (absorving)
        {
            Vector2 direction = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
            Prbody.AddForce(direction.normalized * 60f);
        }
    }

    private void moveboss()
    {
        if(!attacking && cooldown == 0)
        {
            num = Random.Range(0, 10);
            if (num >= 5)
            {
                BossAnim.SetTrigger("absorb");
            }
            else
            {
                BossAnim.SetTrigger("punch");
            }
        }
        else if (!attacking && cooldown > 0)
        {
            print("here");
            if (cooldown < 2)
            {
                cooldown++;
            }
            else
            {
                cooldown = 0;
            }
        }
    }

    public void explode()
    {
            Vector2 pos = new Vector2(player.transform.position.x, player.transform.position.y);
            GameObject explosionInstance = Instantiate(explosion, pos, Quaternion.identity);
    }

    public void absortion()
    {
        absorving = true;
    }
    public void finishAbsortion()
    {
        absorving = false;
    }
    public void finishAtack()
    {
        attacking = false;
        cooldown++;
    }
    public void StartAtack()
    {
        attacking = true;
    }
    public void changeLife(int damage)
    {

        LIFE = LIFE - damage;
        if (LIFE <= 0)
        {
            die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Tocant desde boss");
            player.GetComponent<PlayerController>().lostLife(20);
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

    public void stop()
    {
        StartCoroutine(stopM());
    }

    private IEnumerator stopM()
    {
        rbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
    }
    public void die()
    {
        Destroy(this.gameObject);
    }

    public void play_vacum()
    {
        vacum.Play();
    }
    public void stop_vacum()
    {
        vacum.Stop();
    }
}
