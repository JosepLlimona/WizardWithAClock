using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchInstance : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private bool hit = false;
    // Start is called before the first frame update
    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void DestroyPunch()
    {
        print("destroying");
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !hit)
        {
            Debug.Log("ayuda");
            player.GetComponent<PlayerController>().lostLife(5);
            hit = true;
        }
    }
}
