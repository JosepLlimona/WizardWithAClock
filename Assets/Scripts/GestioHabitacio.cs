using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GestioHabitacio : MonoBehaviour
{
    public Tilemap tilemapColisions;
    public GameObject[] posPortes;
    public GameObject tileParetEsq;
    public GameObject tileParetDreta;
    public GameObject tileParetSuperior;
    public GameObject tileParetInferior;
    public GameObject tilePortaVertical;
    public GameObject tilePortaHoritzontal;

    private int tancades = 0;
    private bool portesTancades = false;

    void Start()
    {
        TancarPortesAleatories();
    }

    void OnTriggerEnter2D(Collider2D personatge){
        if (!portesTancades && personatge.CompareTag("Player")){
            TancarTotesLesPortes();
            portesTancades = true;
        }
    }

    void TancarPortesAleatories(){
        int i = 0;
       while (i < posPortes.Length && tancades < posPortes.Length / 2){
            float nRandom = Random.Range(0f,1f);

            if (nRandom < 0.5f){
                Vector3 posicio = posPortes[i].transform.position;
                string lloc = posPortes[i].name;
                
                GameObject paretPerColocar = ObtenirParetPerNom(lloc);

                if (paretPerColocar != null){
                    Instantiate(paretPerColocar, posicio, Quaternion.identity);
                    tancades++;
                }
            }
            i++;
        }
    }

    void TancarTotesLesPortes(){
        foreach (GameObject porta in posPortes){
            Vector3 posicio = porta.transform.position;
            string lloc = porta.name;
            GameObject portaPerColocar = ObtenirPortaPerNom(lloc);
            if(portaPerColocar != null){
                Instantiate(portaPerColocar, posicio, Quaternion.identity);
            }
        }
    }

    GameObject ObtenirParetPerNom(string lloc){
        if (lloc.Contains("PortaEsq")){
            return tileParetEsq;
        }
        else if (lloc.Contains("PortaDreta")){
            return tileParetDreta;
        }
        else if (lloc.Contains("PortaSuperior")){
            return tileParetSuperior;
        }
        else if (lloc.Contains("PortaInferior")){
            return tileParetInferior;
        }
        return null;
    }


    GameObject ObtenirPortaPerNom(string lloc){
        if (lloc.Contains("PortaEsq") || lloc.Contains("PortaDreta")){
            return tilePortaVertical;
        }
        else if (lloc.Contains("PortaSuperior") || lloc.Contains("PortaInferior")){
            return tilePortaHoritzontal;
        }
        return null;
    }

}
