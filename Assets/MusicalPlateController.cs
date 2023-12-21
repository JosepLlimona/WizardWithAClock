using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalPlateController : PlateController
{
    // Start is called before the first frame update


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
            activarPlaca();
            ReproduirSo();
            Debug.Log("OntriggerEnter -> estic pitjada");

        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (estaPitjada)
        {
            Debug.Log("OnTriggerExit -> Ja no esta PITJADA");
            estaPitjada = false;
            desactivarPlaca();
        }
    }
}
