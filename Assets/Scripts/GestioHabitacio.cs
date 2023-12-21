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
    public GameObject tilePortaEsq;
    public GameObject tilePortaDreta;
    public GameObject tilePortaSuperior;
    public GameObject tilePortaInferior;

    private int tancades = 0;
    private List<Vector3> posPortesTancades = new List<Vector3>();
    private List<GameObject> portaColocada = new List<GameObject>();
    private bool portesTancades = false;
    private int nEnemics = 0;

    void Start()
    {
        TancarPortesAleatories();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            Debug.Log("El jugador ha entrado en la habitacion");
            GenerarEnemics();
            TancarTotesLesPortes();
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
                    posPortesTancades.Add(posicio);
                    tancades++;
                }
            }
            i++;
        }
    }

    void TancarTotesLesPortes(){
        int i = 0;
        while (i <  posPortes.Length){
            Vector3 posicio = posPortes[i].transform.position;
            //if (!posPortesTancades.Contains(posicio)){   ESTO HACE QUE NO SE SUPERPONGAN PAREDES PERO PARA LA DEMO PUEDE DAR ERROR Y NO CERRRAR ALGUNAS PUERTAS
                string lloc = posPortes[i].name;
                GameObject portaPerColocar = ObtenirPortaPerNom(lloc);
                if(portaPerColocar != null){
                    GameObject porta = Instantiate(portaPerColocar, posicio, Quaternion.identity);
                    portaColocada.Add(porta);
                }
            //}
            i++;
        }
    portesTancades = true;
    }


    void ObrirTotesLesPortes(){
        foreach (GameObject porta in portaColocada){
            porta.SetActive(false);
        }
        portesTancades = false;
        portaColocada.Clear();
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
        if (lloc.Contains("PortaEsq")){
            return tilePortaEsq;
        }
        else if (lloc.Contains("PortaDreta")){
            return tilePortaDreta;
        }
        else if (lloc.Contains("PortaSuperior")){
            return tilePortaSuperior;
        }
        else if (lloc.Contains("PortaInferior")){
            return tilePortaInferior;
        }
        return null;
    }

    void GenerarEnemics(){
        Debug.Log("Se generan enemigos");
    }

    void Update(){
        /*if (nEnemics == 0 && portesTancades){
            ObrirTotesLesPortes();
        }*/
        if (Input.GetKeyDown(KeyCode.U)){
            ObrirTotesLesPortes();
        }
    }

}
