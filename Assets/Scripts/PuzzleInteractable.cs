using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;
using Vector3 = UnityEngine.Vector3;

public class PuzzleInteractable : PlateController
{

    public GameObject mirrorPlayer;
    public PlayerController player;


    void Start()
    {

        // Obtenir el component SpriteRenderer
        fader = GetComponent<FadeObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (estaEncesa) { activarPlaca(); }
        else { desactivarPlaca(); }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!estaPitjada)
        {
            estaPitjada = true;

            Debug.Log("OntriggerEnter -> estic pitjada");

        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (estaPitjada)
        {
            Debug.Log("OnTriggerExit -> Ja no esta PITJADA");
            estaPitjada = false;
        }
    }


}
