using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemapMapa;
    public Tile terraPassadis;

    public GameObject[] habPetita;
    private List<int> petitaUsades = new List<int>();
    public GameObject[] habMitjana;
    private List<int> mitjanaUsades = new List<int>();
    public GameObject[] habGran;
    private List<int> granUsades = new List<int>();

    public int habitacionsMapa;
    public float alturaMapa;
    public float ampladaMapa;

    private float petitaAmplada = 7f;
    private float petitaAltura = 7f;
    private float mitjanaAmplada = 8f;
    private float mitjanaAltura = 8f;
    private float granAmplada = 9f;
    private float granAltura = 9f;

    public float separacioHabitacions = 1f;
    private int anterior = 0; 

    private List<List<Vector3>> posPortesPerHabitacio = new List<List<Vector3>>();
    private Dictionary<int, HashSet<int>> portesUsades = new Dictionary<int, HashSet<int>>();


    void Start()
    {
        GenerateMap();
    }

    void GenerateMap(){

        List<Vector3> posUsades = new List<Vector3>();
        List<GestioHabitacio> habitacionsGenerades = new List<GestioHabitacio>();
        float tileSize = tilemapMapa.cellSize.x;
        

        for (int i = 0; i < habitacionsMapa; i++){
                
            int tipus; 
            float ampladaHabitacio;
            float alturaHabitacio;

            ObtenirTipusHabitacio(out tipus, out ampladaHabitacio, out alturaHabitacio);

            float x = Random.Range(-ampladaMapa + ampladaHabitacio, ampladaMapa - ampladaHabitacio);
            float y = Random.Range(-alturaMapa + alturaHabitacio, alturaMapa - alturaHabitacio);

            Vector3 spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);
            
            bool ocupada = PosicioOcupada(spawnPosition, posUsades, tipus);
            int intentos = 0;

            while(ocupada && intentos < 100){ //comporbar que spawnPosition es buida
                x = Random.Range(-ampladaMapa + ampladaHabitacio, ampladaMapa - ampladaHabitacio);
                y = Random.Range(-alturaMapa + alturaHabitacio, alturaMapa - alturaHabitacio);
                spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);

                ocupada = PosicioOcupada(spawnPosition, posUsades, tipus);
                intentos++;
            }

            posUsades.Add(spawnPosition);
            anterior = tipus;

            GameObject habitacioRandom = ObtenirHabitacioRandom(tipus);
            GameObject instanciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);

            instanciarHabitacio.name = i.ToString();

            GestioHabitacio aux = instanciarHabitacio.GetComponent<GestioHabitacio>();
            
            aux.TancarPortesAleatories();
            aux.PosicioPortes();
            

            List<Vector3> posPortes = aux.posicionsPortes;
            posPortesPerHabitacio.Add(posPortes);
        }
        FerPassadis();
    }


    void FerPassadis(){
        for (int i = 0; i < posPortesPerHabitacio.Count; i++){
            for (int j = 0; j < posPortesPerHabitacio.Count; j++){
                if(i != j){
                    int habitacio1 = i;
                    int habitacio2 = j;
                    int rand1 = Random.Range(0,posPortesPerHabitacio[habitacio1].Count);
                    int rand2 = Random.Range(0,posPortesPerHabitacio[habitacio2].Count);

                    Vector3 porta1 = posPortesPerHabitacio[habitacio1][rand1];
                    Vector3 porta2 = posPortesPerHabitacio[habitacio2][rand2];
                    if (!PortaUsada(habitacio1, rand1) && !PortaUsada(habitacio2,rand2)){
                        ConnectarPortes(porta1,porta2);
                        AfegirPortaUsada(habitacio1,rand1);
                        AfegirPortaUsada(habitacio2,rand2);
                    }
                }
            }
        }
    }

    void ConnectarPortes(Vector3 porta1, Vector3 porta2){
        Vector3Int tilePos1 = tilemapMapa.WorldToCell(porta1);
        Vector3Int tilePos2 = tilemapMapa.WorldToCell(porta2);
    }
    

    void ObtenirTipusHabitacio(out int tipus, out float ampladaHabitacio, out float alturaHabitacio){
        tipus = Random.Range(1,4);
            if (anterior == 3 || habGran.Length == granUsades.Count){ //habitacio anterior es gran o llista gran usada
                tipus = Random.Range(1,3);
            }
            else if(anterior == 1 || habPetita.Length == petitaUsades.Count){ //habitacio anterior es petita o llista petita usada
                tipus = Random.Range(2,4);
            }
            else if (habMitjana.Length == mitjanaUsades.Count){ //llista mitjana usada
                tipus = Random.Range(2,4);
                if (tipus == 2){
                    tipus = 1;
                }
            }

            if (tipus == 1){
                ampladaHabitacio = petitaAmplada;
                alturaHabitacio = petitaAltura;
            }
            else if (tipus == 2){
                ampladaHabitacio = mitjanaAmplada;
                alturaHabitacio = mitjanaAltura;
            }
            else{
                ampladaHabitacio = granAmplada;
                alturaHabitacio = granAltura;
            }
    }

    GameObject ObtenirHabitacioRandom(int tipus){
        GameObject habitacio;
        if (tipus ==1){ //agafar habitacio petita

            int index = Random.Range(0, habPetita.Length);
            while (petitaUsades.Contains(index)){
                index = Random.Range(0, habPetita.Length);
            }

            petitaUsades.Add(index);
            habitacio = habPetita[index];
        }

        else if(tipus == 2){ //agafar habitacio mitjana

            int index = Random.Range(0, habMitjana.Length);
            while (mitjanaUsades.Contains(index)){
                index = Random.Range(0, habMitjana.Length);
            }

            mitjanaUsades.Add(index);
            habitacio = habMitjana[index];
        }

        else{ //agafar habitacio gran

            int index = Random.Range(0, habGran.Length);
            while (granUsades.Contains(index)){
                index = Random.Range(0, habGran.Length);
            }

            granUsades.Add(index);
            habitacio = habGran[index];
        }
        return habitacio;
    }

    bool PosicioOcupada(Vector3 pos, List<Vector3> posUsades, int tipus){
        float amplada;
        float altura; 
        
        if (tipus == 1){
            amplada = petitaAmplada;
            altura = petitaAltura;
        }
        else if (tipus == 2){
            amplada = mitjanaAmplada;
            altura = mitjanaAltura;
        }
        else{
            amplada = granAmplada;
            altura = granAltura;
        }

        float tileX = Mathf.Round(pos.x / tilemapMapa.cellSize.x) * tilemapMapa.cellSize.x;
        float tileY = Mathf.Round(pos.y / tilemapMapa.cellSize.x) * tilemapMapa.cellSize.y;

        Vector3 tilePos = new Vector3(tileX, tileY, 0);

        foreach (Vector3 usada in posUsades){
            if (Vector3.Distance(tilePos, usada) < amplada + separacioHabitacions){
                return true;
            }
        }
        return false;
    }

    void AfegirPortaUsada(int habitacio, int porta){
        if (!portesUsades.ContainsKey(habitacio)){
            portesUsades[habitacio] = new HashSet<int>();
        }
        portesUsades[habitacio].Add(porta);
    }

    bool PortaUsada(int habitacio, int porta){
        if (portesUsades.ContainsKey(habitacio)){
            return portesUsades[habitacio].Contains(porta);
        }
        return false;
    }

}
