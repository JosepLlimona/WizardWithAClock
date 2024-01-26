using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GestioHabitacio : MonoBehaviour
{
    public GameObject[] posPortes;
    public GameObject[] enemicsNormal;
    public GameObject[] enemicsGrans;
    public GameObject[] Boss;
    public GameObject tileParetEsq;
    public GameObject tileParetDreta;
    public GameObject tileParetSuperior;
    public GameObject tileParetInferior;
    public GameObject tilePortaEsq;
    public GameObject tilePortaDreta;
    public GameObject tilePortaSuperior;
    public GameObject tilePortaInferior;

    public GameObject Portal;

    private int tancades = 0;
    private List<Vector3> posPortesTancades = new List<Vector3>();
    private List<GameObject> portaColocada = new List<GameObject>();
    private bool portesTancades = false;
    public int nEnemics = 0;

    private List<string> habitacionsVisitades = new List<string>(); 

    private List<GameObject> portesAleatoriesTancades = new List<GameObject>();

    private int nivellActual;

    void Start(){
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        if (mapGenerator != null){
            nivellActual = mapGenerator.nivellActual;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && !portesTancades){
            Debug.Log("El jugador ha entrado en la habitacion");
            if (!habitacionsVisitades.Contains(gameObject.name)){
                TancarTotesLesPortes();
                GenerarEnemics();
            }
        }
    }

    public void PortalActivat(){
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        if(mapGenerator != null){
            mapGenerator.ClearMap();
            if (nivellActual < 4){
                mapGenerator.GenerateMap();
            }
            else{
                mapGenerator.AnarAlLobby();
            }
        }
    }

    public void TancarPortesAleatories(List<Vector3[]> posicionsPortes){
        foreach (GameObject porta in posPortes){
            if (!PosicioEnLlista(porta.transform.position, posicionsPortes)){
                Vector3 posicio = porta.transform.position;
                string lloc = porta.name;
                
                GameObject paretPerColocar = ObtenirParetPerNom(lloc);

                if (paretPerColocar != null){
                    GameObject portaInstanciada = Instantiate(paretPerColocar, posicio, Quaternion.identity);
                    posPortesTancades.Add(posicio);
                    portesAleatoriesTancades.Add(portaInstanciada);
                    tancades++;
                } 
            }
        }
    }

    bool PosicioEnLlista(Vector3 posicio, List<Vector3[]> posicionsPortes){
        foreach(Vector3[] parella in posicionsPortes){
            if (posicio == parella[0] || posicio == parella[1]){
                return true;
            }
        }
        return false;
    }

    void TancarTotesLesPortes(){
        int i = 0;
        while (i <  posPortes.Length){
            Vector3 posicio = posPortes[i].transform.position;

            if (!posPortesTancades.Contains(posicio)){  
                string lloc = posPortes[i].tag;
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
            Destroy(porta);
        }
        portesTancades = false;
        portaColocada.Clear();
        habitacionsVisitades.Add(gameObject.name);
        if (gameObject.CompareTag("Habitacio Boss")){
            GestioPortal gestioP = Portal.GetComponent<GestioPortal>();
            gestioP.MostarPortal();
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
        Debug.Log(nivellActual);
        int nPerGenerar;
        int nEnemicsGrans = 0;
        BoxCollider2D colliderHabitacio = GetComponent<BoxCollider2D>();
        if (gameObject.CompareTag("Habitacio Boss")){
            if (nivellActual == 2 ){
                Vector2 spawnPoint = new Vector2(colliderHabitacio.bounds.center.x, colliderHabitacio.bounds.center.y);
                Instantiate(Boss[0], spawnPoint, Quaternion.identity);
            }
            else if (nivellActual == 4){
                Vector2 spawnPoint = new Vector2(colliderHabitacio.bounds.center.x, colliderHabitacio.bounds.center.y);
                Instantiate(Boss[Boss.Length-1], spawnPoint, Quaternion.identity);
            }
            else{
                ObrirTotesLesPortes();
            }
        }
        else{
            if (gameObject.tag == "HabitacioPetita"){
                nPerGenerar = Random.Range(3, 6);
            }
            else if (gameObject.tag == "HabitacioMitjana"){
                nPerGenerar = Random.Range(5, 9);
            }
            else{
                nPerGenerar = Random.Range(7, 10);
            }
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
    public void ClearHabitacio(){
        EliminarEnemicsInstanciats();
        ObrirTotesLesPortes();
        nEnemics = 0;

        foreach (GameObject porta in portesAleatoriesTancades){
            Destroy(porta);
        }

        tancades = 0;
        posPortesTancades.Clear();
        portaColocada.Clear();
        portesAleatoriesTancades.Clear();
        portesTancades = false;
    }

    public GameObject PortaAleatoria(){
        GameObject porta = posPortes[Random.Range(0,posPortes.Length)];
        return porta;
    }

    public GameObject PortaPerTipus(string tipus){
        foreach(GameObject porta in posPortes){
            if (porta.tag == tipus){
                return porta;
            }
        }
        return null;
    }

    public Vector3 PosicioPortaPerTipus(string tipus){
        Vector3 pos = new Vector3();
        foreach (GameObject porta in posPortes){
           if (porta.tag == tipus){
            pos = porta.transform.position;
           } 
        }
        return pos;
    }

    void EliminarEnemicsInstanciats(){
        GameObject[] enemicsInstanciats = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemic in enemicsInstanciats){
            Destroy(enemic);
            nEnemics--;
        }
    }

    void Update(){
        if (nEnemics <= 0 && portesTancades){
            ObrirTotesLesPortes();
        }
        if (Input.GetKeyDown(KeyCode.O)){
            EliminarEnemicsInstanciats();
        }
    }


}
