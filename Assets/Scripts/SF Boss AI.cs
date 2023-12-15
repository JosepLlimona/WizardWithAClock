using System.Collections;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        AtackOption= Random.Range(0, 20);
        rbody = GetComponent<Rigidbody2D>();
        InvokeRepeating("moveboss", 0f, timemove);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canmove && !atack)
        {
            Vector2 direction = new Vector2(player.transform.position.x + num1 - transform.position.x, player.transform.position.y + num2 - transform.position.y);
            rbody.velocity = direction.normalized * speed;
        }
    }

    private void moveboss()
    {
        num1 = Random.Range(-5, 6);
        num2 = Random.Range(-5, 6);
        canmove = !canmove;
        if (!canmove)
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = 0;
            
            if (AtackOption >= 0 && AtackOption < 2 && !atack)
            {
                
                portalPunch();
            }
            else if (AtackOption >= 2 && AtackOption < 4 && !atack)
            {
                
                TpPunch();
            }
            else if (!atack)
            {

                RayGun();
            }
        }
    }

    private void portalPunch()
    {
        atack = true;
        print("doing portal Punch");
        atack = false;
        AtackOption = Random.Range(0, 11);
    }

    private void TpPunch()
    {
        atack = true;
        print("doing Tp Punch");
        rbody.position = new Vector2(player.transform.position.x + 1 , player.transform.position.y );
        atack = false;
        AtackOption = Random.Range(0, 11);
    }
    private void RayGun()
    {
        atack = true;
        print("doing RayGun");
        atack = false;
        AtackOption = Random.Range(0, 11);
    }
}
