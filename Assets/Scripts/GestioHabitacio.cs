using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GestioHabitacio : MonoBehaviour
{
    public Tilemap tilemapColisions;
    public GameObject[] posPortes;
    public GameObject[] enemicsNormal;
    public GameObject[] enemicsGrans;
    public GameObject Boss;
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
    public int nEnemics = 0;

    void Start()
    {
        TancarPortesAleatories();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && !portesTancades){
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
            if (!posPortesTancades.Contains(posicio)){  
                string lloc = posPortes[i].name;
                GameObject portaPerColocar = ObtenirPortaPerNom(lloc);
                if(portaPerColocar != null){
                    GameObject porta = Instantiate(portaPerColocar, posicio, Quaternion.identity);
                    portaColocada.Add(porta);
                }
            }
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
        int nPerGenerar = Random.Range(5, 10);
        int nEnemicsGrans = 0;
        BoxCollider2D colliderHabitacio = GetComponent<BoxCollider2D>();
        if (gameObject.CompareTag("Habitacio Boss")){
            Vector2 spawnPoint = new Vector2(colliderHabitacio.bounds.center.x, colliderHabitacio.bounds.center.y);
            Instantiate(Boss, spawnPoint, Quaternion.identity);
        }
        else{
            for (int i = 0; i < nPerGenerar; i++){
            
            if (colliderHabitacio != null){
                Vector2 puntRandom = new Vector2(Random.Range(colliderHabitacio.bounds.min.x, colliderHabitacio.bounds.max.x), Random.Range(colliderHabitacio.bounds.min.y, colliderHabitacio.bounds.max.y));
            
                GameObject enemicPerGenerar;
                if (nEnemicsGrans >= nPerGenerar/3){
                    enemicPerGenerar = enemicsNormal[Random.Range(0, enemicsNormal.Length)];
                }
                else{
                    nEnemicsGrans++;
                    enemicPerGenerar = enemicsGrans[Random.Range(0, enemicsGrans.Length)];
                }
                GameObject enemicInstanciat = Instantiate(enemicPerGenerar, puntRandom, Quaternion.identity);
                enemicInstanciat.GetComponent<EnemyLife>().Habitacio = this.gameObject;
                nEnemics++;
            }
        }
        }
        
    }
    

    void Update(){
        Debug.Log("Enemics" + nEnemics);
        if (nEnemics <= 0 && portesTancades){
            ObrirTotesLesPortes();
        }
        if (Input.GetKeyDown(KeyCode.U)){
            ObrirTotesLesPortes();
        }
    }

}
