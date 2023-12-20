using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleController_1 : MonoBehaviour
{
    [SerializeField] private UIController ui;
    [SerializeField] private PuzzleInteractable startPlate;
    [SerializeField] private List<PuzzleInteractuableBehavior> _plaquesPuzzle;
    [SerializeField] private PlayerController mirrorPlayer;
    [SerializeField] private Chest chest;
    private GameObject winnerItem;
    private bool puzzleStarted;
    private int plaquesCompletades = 0;
    private bool puzzleComplet;

    // Start is called before the first frame update
    void Start()
    {
        //winnerItem.gameObject.SetActive(false);
        //chest.DisableChest(); //no volem mostrar el cofre encara
        foreach (var plaques in _plaquesPuzzle)
        {
            plaques.controlarPlaques();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (startPlate.IsON() && !puzzleStarted) { 
            OnPuzzleStart();  
        
        }

        foreach(var plaques in _plaquesPuzzle)
        {
            if (!puzzleComplet)
            {

                plaques.controlarPlaques();
                if (plaques.esCompleta())
                {

                    _plaquesPuzzle.Remove(plaques); //eliminem les plaques completades perque ja no les necessitem
                }
                if(_plaquesPuzzle.Count <= 0)
                {
                    OnPuzzleEnd();
                }
            }
        }
    }

    private void OnPuzzleStart()
    {

        puzzleStarted = true;
        ui.setPuzzleStarted();
        startPlate.FadeOutPlaca();
        //startPlate.gameObject.SetActive(false);


    }
    private void OnPuzzleEnd()
    {
        chest.canOpenChest();
        puzzleComplet = true;
        //AQUI HAURE DE FER SPAWN DEL COFRE
        Debug.Log("PuzzleCompletat");
        chest.EnableChest();
        //Fer desapareixer al mirror player
        mirrorPlayer.disableMirrorPlayer();
        //reproduir so del cofre
    }



}
