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
    private bool canmove = true;
    private bool atack = false;
    private int num1 = 0;
    private int num2 = 0;
    private int AtackOption;
    private Vector2 objective;
    [SerializeField]
    private Animator BossAnim;
    private bool overload = false;

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
        AtackOption = Random.Range(0, 11);
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
        }
        if (!canmove && !overload)
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = 0;

            if (AtackOption >= 0 && AtackOption < 2 && !atack)
            {
                portalPunch();
            }
            else if (AtackOption >= 2 && AtackOption < 20 && !atack)
            {
                TpPunchCharge();
            }
            else if (!atack)
            {

                RayGun();
            }
        }
    }

    private void portalPunch()
    {
        BossAnim.SetBool("PreparingPortalP", false);
        print("doing portal Punch");
    }
    private void TpPunchCharge()
    {
        BossAnim.SetBool("TpCharging",true);
    }
    public void TpPunch()
    {
        BossAnim.SetBool("TpCharging", false);
        BossAnim.SetBool("TpPunchReady", true);
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
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = 0;
        BossAnim.SetBool("TpPunchReady", false);
    }
    private void RayGun()
    {
        print("doing RayGun");
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
        overload = true;
    }
    public void finish_overload()
    {
        overload = false;
    }
}
