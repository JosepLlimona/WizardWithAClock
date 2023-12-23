using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private Rigidbody2D rbody;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Animator BulletAnim;
    private bool alreadyShot = false; 

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    public void shoting()
    {
        if (!alreadyShot)
        {
            //Vector2 pos = rbody.transform.position;
            Vector2 direction = new Vector2(player.transform.position.x - rbody.transform.position.x, player.transform.position.y - rbody.transform.position.y);
            rbody.velocity = direction.normalized * 5;
            alreadyShot = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Player")
        {
            Debug.Log("Tocant desde bala");
            player.GetComponent<PlayerController>().lostLife(10);
            Destroy(this.gameObject);
        }
    }
}
