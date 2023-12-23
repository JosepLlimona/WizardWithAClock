using System;
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

    public static event Action OnShowText;
    public static event Action OnDisableText;
    public static event Action OnPressShowText;
    public static event Action OnPressDisableText;
    // Start is called before the first frame update
    private void Start()
    {
        playerControlls = new PlayerControlls();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleComençat)
        {
            //pressSpaceToRead.showInteractText();
            esPotMostrar = true;
            pressSpaceToRead.gameObject.SetActive(true);
            //OnShowText?.Invoke();
            OnPressShowText?.Invoke();

        }

 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        esPotMostrar = false;
        //OnDisableText?.Invoke();
        OnPressDisableText?.Invoke();
        //pressSpaceToRead.hideInteractText();
        //pressSpaceToRead.hidePuzzleText();
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
