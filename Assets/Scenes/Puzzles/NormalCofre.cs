using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalCofre : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite openSprite, closedSprite;

    [SerializeField] private List<GameObject> WinnerItems;




    private bool isOpen, canOpen;
    private int winnerItemPosition;
    private GameObject winnerItem;

    // Start is called before the first frame update
    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


        Debug.Log("Hauries de veurem");

        canOpen = false;
        isOpen = false;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isOpen)
        {
            Debug.Log("hoila tio");
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            winnerItem = GetWinnerItem();
            Debug.Log(winnerItemPosition);
            Debug.Log(WinnerItems.Count());
            winnerItem.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.4f, 0);
            Instantiate(winnerItem);

            //
            //
            DisableChest();
        }

    }

    public void EnableChest()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

    }

    public void DisableChest()
    {

        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        Debug.Log("shoud fade out");
;

    }

    public GameObject GetWinnerItem()
    {

        winnerItemPosition = Random.Range(0, WinnerItems.Count);
        winnerItem = WinnerItems[winnerItemPosition];
        Instantiate(winnerItem);
        return winnerItem;
    }
    public void canOpenChest() { canOpen = true; }




}