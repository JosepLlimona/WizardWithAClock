using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class SFBossAI : MonoBehaviour
{
    private Rigidbody2D rbody;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int speed = 3;
    [SerializeField]
    private float timemove = 3f;
    [SerializeField]
    private GameObject punch;
    [SerializeField]
    private Transform PunchStart;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform bulletStart;
    private bool canmove = true;
    private bool atack = false;
    private int num1 = 0;
    private int num2 = 0;
    private int AtackOption;
    private Vector2 objective;
    [SerializeField]
    private Animator BossAnim;
    private bool overload = false;
    private float maximumOverload = 20;
    private int numOverloaded = 0;
    private float maximumPortalPunch = 3;
    private int numPortalPunch = 0;

    // Start is called before the first frame update
    void Start()
    {
        AtackOption = Random.Range(0, 20);
        rbody = GetComponent<Rigidbody2D>();
        InvokeRepeating("moveboss", 0f, timemove);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (atack && !overload)
        {
            print("atacking i cant move");
        }
        if (canmove && !atack && !overload)
        {
            Vector2 direction = objective;
            rbody.velocity = direction.normalized * speed;
        }
    }

    private void moveboss()
    {
        AtackOption = Random.Range(0, 15);
        num1 = Random.Range(-5, 6);
        num2 = Random.Range(-5, 6);
        if (num1 < 2 && num1 > -2)
        {
            num1 = 2;
        }
        if (num2 < 2 && num1 > -2)
        {
            num2 = 2;
        }
        objective = new Vector2(player.transform.position.x + num1 - transform.position.x, player.transform.position.y + num2 - transform.position.y);
        if (!atack && !overload)
        {
            canmove = !canmove;
            if (canmove)
            {
                BossAnim.SetBool("moving", true);
            }
            else
            {
                BossAnim.SetBool("moving", false);
            }
        }
        if (!canmove && !overload)
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = 0;

            if (AtackOption >= 0 && AtackOption < 2 && !atack)
            {
                portalPunch();
                BossAnim.SetBool("PreparingPortalP", true);
            }
            else if (AtackOption >= 2 && AtackOption < 4 && !atack)
            {
                TpPunchCharge();
            }
            if (!atack)
            {
                BossAnim.SetTrigger("rasho");
                RayGun();
            }
        }
    }

    private void portalPunch()
    {
        float place = numPortalPunch + 1;
        SummonInstance(PunchStart.transform.position.x + place / 3, PunchStart.transform.position.y + place / 3);
        SummonInstance(PunchStart.transform.position.x - place / 3, PunchStart.transform.position.y + place / 3);
        SummonInstance(PunchStart.transform.position.x + place / 3, PunchStart.transform.position.y - place / 3);
        SummonInstance(PunchStart.transform.position.x - place / 3, PunchStart.transform.position.y - place / 3);

        SummonInstance(PunchStart.transform.position.x, PunchStart.transform.position.y + place / 2);
        SummonInstance(PunchStart.transform.position.x - place / 2, PunchStart.transform.position.y);
        SummonInstance(PunchStart.transform.position.x + place / 2, PunchStart.transform.position.y);
        SummonInstance(PunchStart.transform.position.x, PunchStart.transform.position.y - place / 2);

        if (2 <= numPortalPunch)
        {
            SummonInstance(PunchStart.transform.position.x + place / 2, PunchStart.transform.position.y + place / 4);
            SummonInstance(PunchStart.transform.position.x - place / 4, PunchStart.transform.position.y + place / 2);
            SummonInstance(PunchStart.transform.position.x + place / 4, PunchStart.transform.position.y - place / 2);
            SummonInstance(PunchStart.transform.position.x - place / 2, PunchStart.transform.position.y - place / 4);

            SummonInstance(PunchStart.transform.position.x + place / 4, PunchStart.transform.position.y + place / 2);
            SummonInstance(PunchStart.transform.position.x - place / 2, PunchStart.transform.position.y + place / 4);
            SummonInstance(PunchStart.transform.position.x + place / 2, PunchStart.transform.position.y - place / 4);
            SummonInstance(PunchStart.transform.position.x - place / 4, PunchStart.transform.position.y - place / 2);
        }
    }
    private void SummonInstance(float posx, float posy)
    {
        Vector2 pos = new Vector2(posx, posy);
        GameObject punchInstance = Instantiate(punch, pos, Quaternion.identity);
        punchInstance.GetComponent<Animator>().SetTrigger("Attacking");
    }
    public void add_portal_punch()
    {
        numPortalPunch++;
        if (numPortalPunch >= maximumPortalPunch * speed)
        {
            BossAnim.SetBool("PreparingPortalP", false);
            numPortalPunch = 0;
        }
    }
    private void TpPunchCharge()
    {
        BossAnim.SetBool("TpCharging", true);
    }
    public void TpPunch()
    {
        BossAnim.SetBool("TpPunchReady", true);
        BossAnim.SetBool("TpCharging", false);
    }

    public void punchTp()
    {
        rbody.position = new Vector2(player.transform.position.x - 1, player.transform.position.y);
    }
    public void punchDash()
    {
        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        rbody.velocity = direction.normalized * speed * 4;
    }
    public void end_punch()
    {
        BossAnim.SetBool("TpPunchReady", false);
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = 0;
    }
    private void RayGun()
    {
        print("doing RayGun");
        Vector2 pos = bulletStart.transform.position;
        GameObject BulletInstance = Instantiate(bullet, pos, Quaternion.identity);
        BulletInstance.GetComponent<Animator>().SetTrigger("Shot");
        Vector2 direction = new Vector2(player.transform.position.x - bulletStart.transform.position.x, player.transform.position.y - bulletStart.transform.position.y);
        Rigidbody2D rbodyBullet;
        rbodyBullet = bullet.GetComponent<Rigidbody2D>();
        rbodyBullet.velocity = direction.normalized * 5;
    }

    public void startAtack()
    {
        atack = true;
    }
    public void finishAtack()
    {
        atack = false;
    }
    public void start_overload()
    {
        BossAnim.SetBool("overload", true);
        overload = true;
    }
    public void add_overload()
    {
        numOverloaded++;
        if (numOverloaded >= maximumOverload / speed)
        {
            finish_overload();
        }
    }
    public void finish_overload()
    {
        atack = false;
        BossAnim.SetBool("overload", false);
        overload = false;
        numOverloaded = 0;
    }
}
