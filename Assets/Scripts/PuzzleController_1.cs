using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleController_1 : PuzzleController
{

    [SerializeField] private List<PuzzleInteractuableBehavior> _plaquesPuzzle;
    [SerializeField] private PlayerController mirrorPlayer;

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

        if (startPlate.IsON() && !puzzleStarted)
        {
            OnPuzzleStart();

        }

        if (puzzleStarted)
        {
            ControlarPlaquesPuzzle();
        }
    }

    private void OnMirrorPuzzleEnd()
    {
        OnPuzzleEnd();
        mirrorPlayer.disableMirrorPlayer();
        //reproduir so del cofre
    }

    private void ControlarPlaquesPuzzle()
    {
        foreach (var plaques in _plaquesPuzzle)
        {
            if (!puzzleComplet)
            {

                plaques.controlarPlaques();
                if (plaques.esCompleta())
                {

                    _plaquesPuzzle.Remove(plaques); //eliminem les plaques completades perque ja no les necessitem
                }
                if (_plaquesPuzzle.Count <= 0)
                {
                    OnMirrorPuzzleEnd();
                }
            }
        }
    }


}
