using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PlateController : MonoBehaviour
{

    protected bool estaPitjada = false;  // Estat de si est� pitjada
    [SerializeField] protected bool estaEncesa;   // Estat de si est� en ON
    [SerializeField] private bool PlacaIniciPuzzle; //Per controlar la placa de inici i no crear un script a part
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;  // AudioSource per reproduir el so
    protected FadeObject fader;
    // Start is called before the first frame update
    [SerializeField] private Sprite spriteON;  // Assenyala l'sprite quan la placa est� en ON
    [SerializeField] private Sprite spriteOFF;  // Assenyala l'sprite quan la placa est� en OFF

    
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hola tio");
        if (PlacaIniciPuzzle)
        {
            fader = GetComponent<FadeObject>();
            activarPlaca();
            
        }
        else
        {
            activarPlaca();
            desactivarPlaca();
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
