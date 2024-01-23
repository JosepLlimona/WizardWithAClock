using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemapMapa;
    public Tilemap tilemapDecoracio;
    public Tilemap tilemapCollision;
    public Tile terraPassadis;
    public Tile paretEsq;
    public Tile paretDrt;
    public Tile paretInf;
    public Tile paretSup;
    public Tile paretSupD;
    public Tile error;

    public GameObject[] habPetita;
    private List<int> petitaUsades = new List<int>();
    public GameObject[] habMitjana;
    private List<int> mitjanaUsades = new List<int>();
    public GameObject[] habGran;
    private List<int> granUsades = new List<int>();

    public GameObject habitacioSpawn;

    public int habitacionsMapa;

    private static float tileSize = 0.32f;

    private float alturaMapa = 27;
    private float ampladaMapa = 27;


    private float petitaAmplada = 5;
    private float petitaAltura = 5;
    private float mitjanaAmplada = 7;
    private float mitjanaAltura = 7;
    private float granAmplada = 8;
    private float granAltura = 8;

    private float separacioHabitacions = 2;
    private int anterior = 0; 

    private List<List<GameObject>> posPortesPerHabitacio = new List<List<GameObject>>();
    private Dictionary<int, HashSet<int>> portesUsades = new Dictionary<int, HashSet<int>>();

    private List<GameObject> habitacionsInstanciades = new List<GameObject>();


    void Start()
    {
        GenerateMap();
    }

    void GenerateMap(){

        GenerarHabitacionsEspecials();

        for (int i = 0; i < habitacionsMapa; i++){
                
            int tipus; 
            float ampladaHabitacio;
            float alturaHabitacio;

            ObtenirTipusHabitacio(out tipus, out ampladaHabitacio, out alturaHabitacio);

            float x, y;
            Vector3 spawnPosition;

            
            x = Random.Range(-ampladaMapa + ampladaHabitacio, ampladaMapa - ampladaHabitacio);
            y = Random.Range(-alturaMapa + alturaHabitacio, alturaMapa - alturaHabitacio);

            spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);
            
            bool ocupada = PosicioOcupada(spawnPosition, habitacionsInstanciades, tipus);
            int intentos = 0;

            while(ocupada && intentos < 100){ //comporbar que spawnPosition es buida
                x = Random.Range(-ampladaMapa + ampladaHabitacio, ampladaMapa - ampladaHabitacio);
                y = Random.Range(-alturaMapa + alturaHabitacio, alturaMapa - alturaHabitacio);
                spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);

                ocupada = PosicioOcupada(spawnPosition, habitacionsInstanciades, tipus);
                intentos++;
            }

            anterior = tipus;

            GameObject habitacioRandom = ObtenirHabitacioRandom(tipus);
            GameObject instanciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);

            habitacionsInstanciades.Add(instanciarHabitacio);

            instanciarHabitacio.name = i.ToString();

            GestioHabitacio aux = instanciarHabitacio.GetComponent<GestioHabitacio>();
            
            aux.TancarPortesAleatories();
            aux.PosicioPortes();
            

            List<GameObject> posPortes = aux.posicionsPortes;
            posPortesPerHabitacio.Add(posPortes);
        }
        FerPassadis();
    }

    void GenerarHabitacionsEspecials(){

        float x, y;
        Vector3 spawnPosition;

        
        x = Random.Range(-ampladaMapa + petitaAmplada, ampladaMapa - petitaAmplada);
        y = Random.Range(-alturaMapa + petitaAltura, alturaMapa - petitaAmplada);

        spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);
            
        bool ocupada = PosicioOcupada(spawnPosition, habitacionsInstanciades, 1);
        int intentos = 0;

        while(ocupada && intentos < 100){ //comporbar que spawnPosition es buida
            x = Random.Range(-ampladaMapa + petitaAmplada, ampladaMapa - petitaAmplada);
            y = Random.Range(-alturaMapa + petitaAltura, alturaMapa - petitaAltura);
            spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);

            ocupada = PosicioOcupada(spawnPosition, habitacionsInstanciades, 1);
            intentos++;
        }


        GameObject instanciarHabitacio = Instantiate(habitacioSpawn, spawnPosition, Quaternion.identity);
        habitacionsInstanciades.Add(instanciarHabitacio);

        GestioSpawn aux = instanciarHabitacio.GetComponent<GestioSpawn>();
            
        aux.TancarPortesAleatories();
        aux.PosicioPortes();
            
        List<GameObject> posPortes = aux.posicionsPortes;
        posPortesPerHabitacio.Add(posPortes);
    }


    void FerPassadis(){
        for (int i = 0; i < posPortesPerHabitacio.Count; i++){
            int habitacio = i;
            for (int j = 0; j < posPortesPerHabitacio[i].Count; j++){
                GameObject porta1 = posPortesPerHabitacio[i][j];
                int[] indexPorta2 = TrobarPortaMesPropera(habitacio,porta1);
                GameObject porta2 = new GameObject();
                if (indexPorta2[0] != -1 && indexPorta2[1] != -1){
                    porta2 = posPortesPerHabitacio[indexPorta2[0]][indexPorta2[1]];
                     if (porta1.transform.position != Vector3.zero && porta2.transform.position != Vector3.zero){
                        ConnectarPortes(porta1,porta2);
                        AfegirPortaUsada(habitacio, j);
                        AfegirPortaUsada(indexPorta2[0], indexPorta2[1]);
                    }
                }

               
            }
        }
    }

    int[] TrobarPortaMesPropera(int habitacio, GameObject portaRef){
        int[] indexPortaSeleccionada = {-1, -1};
        float distanciaMinima = float.MaxValue;

        for (int i = 0; i < posPortesPerHabitacio.Count; i++){
            if (i != habitacio){
                List<GameObject> posPortesHabitacio = posPortesPerHabitacio[i];
                foreach (GameObject porta in posPortesHabitacio){
                    int indexPorta = posPortesPerHabitacio[i].IndexOf(porta);
                    if (!portesUsades.ContainsKey(i) || (portesUsades.ContainsKey(i) && !portesUsades[i].Contains(indexPorta))){
                        float distancia = Vector3.Distance(portaRef.transform.position, porta.transform.position);
                        if (distancia < distanciaMinima && PortesAlineades(portaRef, porta)){
                            distanciaMinima = distancia;
                            indexPortaSeleccionada[0] = i;
                            indexPortaSeleccionada[1] = indexPorta;
                        }
                    }
                }
            }
        }
        return indexPortaSeleccionada;
    }

    bool PortesAlineades(GameObject porta1, GameObject porta2){
        if ((porta1.tag == "PortaEsq" && porta2.tag == "PortaDreta") || (porta1.tag == "PortaDreta" && porta2.tag == "PortaEsq") || (porta1.tag == "PortaDreta" && porta2.tag == "PortaDreta") || (porta1.tag == "PortaEsq" && porta2.tag == "PortaEsq")){
            return true;
        }
        else if ((porta1.tag == "PortaSuperior" && porta2.tag == "PortaInferior") || (porta1.tag == "PortaInferior" && porta2.tag == "PortaSuperior") || (porta1.tag == "PortaInferior" && porta2.tag == "PortaInferior") || (porta1.tag == "PortaSuperior" && porta2.tag == "PortaSuperior")){
            return true;
        }
        else{
            return false;
        }
    }

    void ConnectarPortes(GameObject porta1, GameObject porta2){
        Vector3Int tilePos1 = tilemapMapa.WorldToCell(porta1.transform.position);
        Vector3Int tilePos2 = tilemapMapa.WorldToCell(porta2.transform.position);

        int startX = tilePos1.x;
        int startY = tilePos1.y;
        int endX = tilePos2.x;
        int endY = tilePos2.y;

        tilemapMapa.SetTile(tilePos1, error);
        tilemapMapa.SetTile(tilePos2, error);
        int directionX = (startX < endX) ? 1 : -1;
        int directionY = (startY < endY) ? 1 : -1;

        for (int x = startX; x != endX; x += directionX) {
            Vector3Int tilePos = new Vector3Int(x, startY, 0);
            if (!PosicioTileMapOcupada(tilePos) && !PosicioOcupada(new Vector3(x, startY, 0), habitacionsInstanciades, 0)) {
                ColocarPassadisHoritzontal(tilePos);
            }
        }

        for (int y = startY; y != endY; y += directionY) {
            Vector3Int tilePos = new Vector3Int(endX, y, 0);
            if (!PosicioTileMapOcupada(tilePos) && !PosicioOcupada(new Vector3(endX, y, 0), habitacionsInstanciades, 0)) {
                ColocarPassadisVertical(tilePos);
            }
        }  
    }
    
    void ColocarPassadisHoritzontal(Vector3Int tilePos){
        tilemapMapa.SetTile(tilePos, terraPassadis);
        tilemapCollision.SetTile(tilePos, paretInf);

        /*Vector3Int paretInfPos = new Vector3Int(tilePos.x, tilePos.y + 1, 0);
        tilemapMapa.SetTile(paretInfPos, terraPassadis);

        Vector3Int paretSupDPos = new Vector3Int(tilePos.x, tilePos.y + 2, 0);
        tilemapDecoracio.SetTile(paretSupDPos, paretSupD);
        tilemapMapa.SetTile(paretSupDPos, terraPassadis);

        Vector3Int paretSupPos = new Vector3Int(tilePos.x, tilePos.y + 3, 0);
        tilemapCollision.SetTile(paretSupPos, paretSup);*/
    }

    void ColocarPassadisVertical(Vector3Int tilePos){
        tilemapMapa.SetTile(tilePos, terraPassadis);
        tilemapCollision.SetTile(tilePos, paretEsq);

        /*Vector3Int paretEsqPos = new Vector3Int(tilePos.x + 1 , tilePos.y, 0);
        
        tilemapMapa.SetTile(paretEsqPos, terraPassadis);

        Vector3Int paretDrtPos = new Vector3Int(tilePos.x + 2, tilePos.y, 0);
        tilemapCollision.SetTile(paretDrtPos, paretDrt);
        tilemapMapa.SetTile(paretDrtPos, terraPassadis);*/
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

    bool PosicioOcupada(Vector3 pos, List<GameObject> habitacionsInstanciades, int tipus){
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

        foreach (GameObject habitacio in habitacionsInstanciades){
            
            float minX = habitacio.transform.position.x - separacioHabitacions;
            float maxX = habitacio.transform.position.x + granAmplada + separacioHabitacions;
            float minY = habitacio.transform.position.y - separacioHabitacions;
            float maxY = habitacio.transform.position.y + granAltura;

            if (pos.x >= minX && pos.x <= maxX && pos.y >= minY && pos.y <= maxY){
                return true;
            }
            if (pos.x + amplada >= minX && pos.x + amplada <= maxX && pos.y >= minY && pos.y <= maxY){
                return true;
            }
            if (pos.x >= minX && pos.x <= maxX && pos.y + altura >= minY && pos.y + altura <= maxY){
                return true;
            }
            if (pos.x + amplada >= minX && pos.x + amplada <= maxX && pos.y + altura >= minY && pos.y + altura <= maxY){
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

    bool PosicioTileMapOcupada(Vector3Int pos){
        int i = -3;
        bool HiHaPassadis = false;
        while(i <=3 && !HiHaPassadis){
            TileBase tile = tilemapMapa.GetTile(pos + new Vector3Int(i,0,0));
            TileBase tile1= tilemapMapa.GetTile(pos + new Vector3Int(0,i,0));
            TileBase tile2 = tilemapMapa.GetTile(pos + new Vector3Int(i,i,0));
            TileBase tile3 = tilemapMapa.GetTile(pos + new Vector3Int(i,-i,0));
            if (tile != null || tile1 != null || tile2 != null || tile3 != null){
                HiHaPassadis = true;
            }
            i++;
        }
        return HiHaPassadis;
    }

    void ClearMap(){
        
        GestioSpawn auxS = habitacionsInstanciades[0].GetComponent<GestioSpawn>();
        auxS.ClearSpawn();
        Destroy(habitacionsInstanciades[0]);

        for (int i = 1; i < habitacionsInstanciades.Count; i++){
            GestioHabitacio aux = habitacionsInstanciades[i].GetComponent<GestioHabitacio>();
            if (aux != null){
                aux.ClearHabitacio();
                Destroy(habitacionsInstanciades[i]);
            }
            
        }

        tilemapMapa.ClearAllTiles();
        tilemapDecoracio.ClearAllTiles();
        tilemapCollision.ClearAllTiles();

        petitaUsades.Clear();
        mitjanaUsades.Clear();
        granUsades.Clear();
        posPortesPerHabitacio.Clear();
        portesUsades.Clear();
        habitacionsInstanciades.Clear();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            ClearMap();
            GenerateMap();
        }
    }

}
