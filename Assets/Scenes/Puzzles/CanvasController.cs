using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CanvasController canva;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showInteractText()
    {



            this.gameObject.SetActive(true);
      
    }

    public void hideInteractText()
    {

            gameObject.SetActive(false);
       
    }
    public void showPuzzleText()
    {


        canva.showInteractText();
            
        
    }
    public void hidePuzzleText()
    {
        canva.hideInteractText();
    }

    public bool isInstructionActive()
    {
        return canva.isActive();
    }
    public bool isActive() { return gameObject.activeInHierarchy == true; }
}



