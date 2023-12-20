using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIActiveController : MonoBehaviour
{
    [SerializeField] private CanvasController canva;
    private PlayerControlls playerControlls;
    private bool esPotMostrar = false;
    private bool mostrat = false;
    // Start is called before the first frame update
    private void Start()
    {
        playerControlls = new PlayerControlls();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        canva.showInteractText();
        esPotMostrar = true;

 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        esPotMostrar = false;
        canva.hideInteractText();
        canva.hidePuzzleText();
    }
    public void onShowCanva()
    {

        if (esPotMostrar && !mostrat)
        {
            canva.showPuzzleText();
            mostrat = true;
        }
        else if (esPotMostrar && mostrat)
        {
            canva.hidePuzzleText();
            mostrat = false;
        }

    }
}
