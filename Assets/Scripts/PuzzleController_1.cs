using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleController_1 : MonoBehaviour
{
    [SerializeField] private List<PuzzleInteractuableBehavior> _plaquesPuzzle;
    [SerializeField] private bool hola;
    private int plaquesCompletades = 0;
    private bool puzzleComplet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                    puzzleComplet = true;
                    OnPuzzleEnd();
                }
            }
        }
    }


    private void OnPuzzleEnd()
    {
        //AQUI HAURE DE FER SPAWN DEL COFRE
        Debug.Log("PuzzleCompletat");
        //reproduir so del cofre
    }



}
