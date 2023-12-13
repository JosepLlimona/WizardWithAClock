using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;



public class PuzzleInteractable : MonoBehaviour
{
    [SerializeField] private int id;  // Identificador de la placa
    private bool estaPitjada = false;  // Estat de si est� pitjada
    [SerializeField] private bool estaEncesa;   // Estat de si est� en ON
    private SpriteRenderer spriteRenderer;
    public Sprite spriteON;  // Assenyala l'sprite quan la placa est� en ON
    public Sprite spriteOFF;  // Assenyala l'sprite quan la placa est� en OFF
    public AudioSource audioSource;  // AudioSource per reproduir el so
    private void Start()
    {

        // Obtenir el component SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); 

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

    // M�tode per comprovar si la placa est� en ON
    public bool IsON()
    {
        return estaEncesa;
    }

    // M�tode per comprovar si la placa est� en OFF
    public bool IsOFF()
    {
        return !estaEncesa;
    }

    // M�tode per comprovar si la placa est� sent pitjada
    public bool EstaSiguentPitjada()
    {
        return estaPitjada;
    }

    public void activarPlaca()
    {
        spriteRenderer.sprite = spriteON;
        estaEncesa = true;
    }

    public void desactivarPlaca()
    {
        spriteRenderer.sprite = spriteOFF;
        estaEncesa = false;
    }


    // M�tode per reproduir l'AudioSource
    public void ReproduirSo()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
