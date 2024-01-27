using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleController_1 : PuzzleController
{
    [SerializeField] private List<PuzzleInteractuableBehavior> _plaquesPuzzle;
    [SerializeField] private PlayerController mirrorPlayer;
    [SerializeField] private PlayerController player;
    [SerializeField] private AudioSource audio;
    //[SerializeField] private CanvasController uis;

    private bool mirrorPlayerInstantiated = false;

    // Start is called before the first frame update
    void Start()
    {
        //uis = GetComponent<CanvasController>();
        mirrorPlayer.gameObject.SetActive(false);

        foreach (var plaques in _plaquesPuzzle)
        {
            plaques.controlarPlaques();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlate.IsON() && !puzzleStarted && !puzzleComplet)
        {
            //uis.hideInteractText();
            OnPuzzleStart();

            if (!mirrorPlayerInstantiated)
            {
                Debug.Log("holaaa");
                mirrorPlayerInstantiated = true;
                /**
                 * 
                 * 
                Vector3 mirrorPlayerPosition = new Vector3(
                    -0.072f,
                    -0.3f,
                    player.transform.position.z
                );

                 */
                Vector3 mirrorPlayerPosition = new Vector3(
                     player.transform.position.x,
                     -player.transform.position.y+0.6f,
                 player.transform.position.z
);

                mirrorPlayer.transform.position = mirrorPlayerPosition;
                mirrorPlayer.enabled = true;
                mirrorPlayer.gameObject.SetActive(true);
            }
        }

        ControlarPlaquesPuzzle();
    }

    private void OnMirrorPuzzleEnd()
    {
        OnPuzzleEnd();
        mirrorPlayer.disableMirrorPlayer();
    }

    private void ControlarPlaquesPuzzle()
    {
        foreach (var plaques in _plaquesPuzzle.ToList()) // Usar ToList para evitar problemas al modificar la lista
        {
            if (!puzzleComplet)
            {
                plaques.controlarPlaques();

                if (plaques.esCompleta())
                {
                    audio.Play();
                    _plaquesPuzzle.Remove(plaques);
                }

                if (_plaquesPuzzle.Count <= 0)
                {
                    OnMirrorPuzzleEnd();
                }
            }
        }
    }
}

