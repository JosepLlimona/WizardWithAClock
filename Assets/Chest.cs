using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite openSprite, closedSprite;

    [SerializeField] private List<GameObject> WinnerItems;
    [SerializeField] private FadeObject fader;


    private bool isOpen,canOpen;
    private int winnerItemPosition;
    private GameObject winnerItem;
    
    // Start is called before the first frame update
    public void Start()
    {
        // winnerItem.SetActive(false);
        //gameObject.transform.position = new Vector3(0,0.5f,0);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        canOpen = false;
        isOpen = false;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isOpen && canOpen)
        {
            Debug.Log("hoila tio");
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            winnerItem = GetWinnerItem();
            Debug.Log(winnerItemPosition);
            Debug.Log(WinnerItems.Count());
            winnerItem.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+0.7f, 0);
            Instantiate(winnerItem);
          
            //
            //
            DisableChest();
        }

    }

    public void EnableChest()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        fader.startFadingIn();
        //transform.position = Vector3.zero;
    }

    public void DisableChest()
    {
        //fader.startFadingOut();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        Debug.Log("shoud fade out");
        //Destroy(this.gameObject);
        //gameObject.SetActive(false);
 
    }

    public GameObject GetWinnerItem()
    {

        winnerItemPosition = Random.Range(0, WinnerItems.Count);
        winnerItem = WinnerItems[winnerItemPosition];
        return winnerItem;
    }
    public void canOpenChest() { canOpen = true; }

    
}