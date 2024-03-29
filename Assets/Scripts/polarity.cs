using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class polarity : MonoBehaviour, EnemyLife
{
    private float Direction = 4;
    private Rigidbody2D rbody;
    private GameObject player;
    [SerializeField]
    private GameObject counterpart;
    [SerializeField]
    private int speed = 3;
    [SerializeField]
    private float timemove = 3f;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform bulletStart;
    private Vector2 objective;
    [SerializeField]
    private Animator BossAnim;
    private bool armor = false;
    private int LIFE = 250;
    private int bulletAmount = 1;
    private int damageTaken = 0;
    private bool canMove = true;
    [SerializeField]
    private AudioSource shieldSound;
    [SerializeField]
    private AudioSource raygunSound;
    [SerializeField]
    Slider life;

    public GameObject habitacio;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        //InvokeRepeating("moveboss", 0f, timemove);
        InvokeRepeating("RayGun", 0f, 1f);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }
    public void jumping()
    {
        Vector2 direction = objective;
        rbody.velocity = direction.normalized * speed;
    }

    public void set_counterpart(GameObject counter)
    {
        counterpart = counter;
    }
    private void moveboss()
    {
        if (canMove)
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = 0;
            if (Direction >= 0)
            {
                Direction = -4;
            }
            else if (Direction < 0)
            {
                Direction = 4;
            }
            objective = new Vector2(0, Direction + transform.position.y);
            jumping();
        }
    }

    private void RayGun()
    {
        if (armor)
        {
            raygunSound.Play();
            print("doing RayGun");
            Vector2 pos = bulletStart.transform.position;
            GameObject BulletInstance = Instantiate(bullet, pos, Quaternion.identity);
            BulletInstance.GetComponent<bulletScript>().setPlayer(player);
            //BulletInstance.GetComponent<Animator>().SetTrigger("Shot");
        }
    }

    public void perd_paralel(float mal)
    {
        life.value -= mal;
    }
    public void changeLife(int damage)
    {
        if (!armor)
        {
            life.value -= damage;
            damageTaken += damage;
            counterpart.GetComponent<polarity_blue>().perd_paralel(damage);
        }
        if (damageTaken > 50)
        {
            activateArmor();
            damageTaken = 0;
        }
        if (life.value <= 0)
        {
            die();
            counterpart.GetComponent<polarity_blue>().die();
            habitacio.GetComponent<GestioHabitacio>().nEnemics--;
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
    public void activateArmor()
    {
        BossAnim.SetTrigger("ChargingArmor");
        armor = true;
        counterpart.GetComponent<polarity_blue>().deactivateArmor();
    }
    public void play_armor()
    {
        shieldSound.Play();
    }
    public void deactivateArmor()
    {
        BossAnim.SetTrigger("DesArmor");
        armor = false;

    }

    public void stop()
    {
        StartCoroutine(stopM());
    }

    private IEnumerator stopM()
    {
        canMove = false;
        rbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        canMove = true;
    }
    public void die()
    {
        Destroy(this.gameObject);
    }
}