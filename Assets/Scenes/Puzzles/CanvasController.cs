using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CanvasController canva;
    public static event Action onShowPuzzle;
    public static event Action onHidePuzzle;

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
        onShowPuzzle?.Invoke();

            
        
    }
    public void hidePuzzleText()
    {
        onHidePuzzle?.Invoke();


        //canva.hideInteractText();
    }

    public bool isInstructionActive()
    {
        return canva.isActive();
    }
    public bool isActive() { return gameObject.activeInHierarchy == true; }
}



