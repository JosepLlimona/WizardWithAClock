using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Num2 : MonoBehaviour
{
    private Rigidbody2D rbody;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private bool alreadyShot = false;

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterDelay(3f));
    }

    private void FixedUpdate()
    {
        if (!alreadyShot)
        {
            Vector2 direction = new Vector2(player.transform.position.x - rbody.transform.position.x, player.transform.position.y - rbody.transform.position.y);
            rbody.velocity = direction.normalized * 2.5f;
            alreadyShot = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Tocant desde bala");
            player.GetComponent<PlayerController>().lostLife(5);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
