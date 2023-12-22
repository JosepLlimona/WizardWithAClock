using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PlateController : MonoBehaviour
{

    protected bool estaPitjada = false;  // Estat de si està pitjada
    [SerializeField] protected bool estaEncesa;   // Estat de si està en ON
    [SerializeField] private bool PlacaIniciPuzzle; //Per controlar la placa de inici i no crear un script a part
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;  // AudioSource per reproduir el so
    protected FadeObject fader;
    // Start is called before the first frame update
    [SerializeField] protected Sprite spriteON;  // Assenyala l'sprite quan la placa està en ON
    [SerializeField] protected Sprite spriteOFF;  // Assenyala l'sprite quan la placa està en OFF

    
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
            //desactivarPlaca();
        }
    }

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
        Debug.Log("FicantSpriteOn");
        estaEncesa = true;
    }

    public void desactivarPlaca()
    {

        spriteRenderer.sprite = spriteOFF;
        Debug.Log("FicantSpriteOFF");
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

    public void FadeOutPlaca()
    {
        fader.startFadingOut();
    }

    public void sonarPlaca()
    {
        StartCoroutine(ferSonarPlaca());
    }
    IEnumerator ferSonarPlaca()
    {
        activarPlaca();
        ReproduirSo();
        yield return new WaitForSeconds(0.5f);
        desactivarPlaca();
    }
}
