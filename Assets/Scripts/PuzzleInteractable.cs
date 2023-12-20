using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;
using Vector3 = UnityEngine.Vector3;

public class PuzzleInteractable : MonoBehaviour
{
    [SerializeField] private int id;  // Identificador de la placa
    private bool estaPitjada = false;  // Estat de si est� pitjada
    [SerializeField] private bool estaEncesa;   // Estat de si est� en ON
    [SerializeField] private bool PlacaIniciPuzzle; //Per controlar la placa de inici i no crear un script a part
    private SpriteRenderer spriteRenderer;
    public Sprite spriteON;  // Assenyala l'sprite quan la placa est� en ON
    public Sprite spriteOFF;  // Assenyala l'sprite quan la placa est� en OFF
    public AudioSource audioSource;  // AudioSource per reproduir el so
    public GameObject mirrorPlayer;
    public PlayerController player;
    [SerializeField] private FadeObject fader;

    private void Start()
    {

        // Obtenir el component SpriteRenderer
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
        if (PlacaIniciPuzzle)
        {
            //
            activarPlaca();
            if(mirrorPlayer.activeInHierarchy == false)
            {

                mirrorPlayer.transform.position = new Vector3(
                    player.transform.position.x,
                     -player.transform.position.y, // Inverteix la posici� y del playe
                player.transform.position.z);
                mirrorPlayer.SetActive(true);   
    
            }

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

    public void FadeOutPlaca()
    {
        fader.startFadingOut();
    }
}
