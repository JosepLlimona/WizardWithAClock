using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalPlateController : PlateController
{
    // Start is called before the first frame update
    [SerializeField] private int idPlaca;
    public static event Action<int> OnMusicalPlatePressed;

    void Start()
    {

        // Obtenir el component SpriteRenderer
        fader = GetComponent<FadeObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (estaEncesa) { activarPlaca(); }
        else { desactivarPlaca(); }

    }


    private void OnEnable()
    {
        PuzzleMusicPlatesController.OnPlayerWrong += sonarPlaca;
    }
    private void OnDisable()
    {
        PuzzleMusicPlatesController.OnPlayerWrong -= sonarPlaca;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!estaPitjada)
        {
            estaPitjada = true;
            activarPlaca();
            //spriteRenderer.sprite = spriteON;
            ReproduirSo();
            //desactivarPlaca();
            //spriteRenderer.sprite = spriteOFF;
            if (OnMusicalPlatePressed != null) //invoco el event onmusicalplatepressed quan pitjo una placa
            {
                OnMusicalPlatePressed.Invoke(idPlaca);
            }
            //Debug.Log("OntriggerEnter -> estic pitjada");

        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (estaPitjada)
        {
            //Debug.Log("OnTriggerExit -> Ja no esta PITJADA");
            estaPitjada = false;
            desactivarPlaca();
        }
    }

    public int getIdPlaca() { return idPlaca; }
    public bool esMateixaPlacaPitjada(int idPlaca_PlacaQueHauriaDeSonar)
    {
        return idPlaca == idPlaca_PlacaQueHauriaDeSonar;
    }
}
