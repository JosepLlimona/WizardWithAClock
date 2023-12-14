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

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        InvokeRepeating("movebitch", 0f, timemove);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canmove)
        {
            Vector2 direction = player.transform.position - transform.position;
            rbody.velocity = direction.normalized * speed;
        }
        else
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = 0;
        }
    }

    private void movebitch()
    {
        canmove = !canmove;
    }
}
