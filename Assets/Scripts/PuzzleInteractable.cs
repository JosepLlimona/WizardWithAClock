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
    private bool estaPitjada = false;  // Estat de si està pitjada
    [SerializeField] private bool estaEncesa;   // Estat de si està en ON
    private SpriteRenderer spriteRenderer;
    public Sprite spriteON;  // Assenyala l'sprite quan la placa està en ON
    public Sprite spriteOFF;  // Assenyala l'sprite quan la placa està en OFF
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

    // Mètode per comprovar si la placa està en ON
    public bool IsON()
    {
        return estaEncesa;
    }

    // Mètode per comprovar si la placa està en OFF
    public bool IsOFF()
    {
        return !estaEncesa;
    }

    // Mètode per comprovar si la placa està sent pitjada
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


    // Mètode per reproduir l'AudioSource
    public void ReproduirSo()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
