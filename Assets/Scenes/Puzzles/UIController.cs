using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private CanvasController pressSpaceToRead;
    private PlayerControlls playerControlls;
    private bool esPotMostrar = false;
    private bool mostrat = false;
    private bool puzzleComençat = false;
    // Start is called before the first frame update
    private void Start()
    {
        playerControlls = new PlayerControlls();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleComençat)
        {
            pressSpaceToRead.showInteractText();
            esPotMostrar = true;

        }

 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        esPotMostrar = false;
        pressSpaceToRead.hideInteractText();
        pressSpaceToRead.hidePuzzleText();
    }
    public void onShowCanva()
    {

        if (esPotMostrar && !mostrat)
        {
            pressSpaceToRead.showPuzzleText();
            mostrat = true;
        }
        else if (esPotMostrar && mostrat)
        {
            pressSpaceToRead.hidePuzzleText();
            mostrat = false;
        }

    }
    public void setPuzzleStarted()
    {
        puzzleComençat = true;
        esPotMostrar = false;
        pressSpaceToRead.hideInteractText();
        pressSpaceToRead.hidePuzzleText();

    }
}
