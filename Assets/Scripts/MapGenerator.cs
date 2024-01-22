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

    private List<List<Vector3>> posPortesPerHabitacio = new List<List<Vector3>>();
    private Dictionary<int, HashSet<int>> portesUsades = new Dictionary<int, HashSet<int>>();

    private List<Vector3> posUsades = new List<Vector3>();

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

            habitacionsInstanciades.Add(instanciarHabitacio);

            instanciarHabitacio.name = i.ToString();

            GestioHabitacio aux = instanciarHabitacio.GetComponent<GestioHabitacio>();
            
            aux.TancarPortesAleatories();
            aux.PosicioPortes();
            

            List<Vector3> posPortes = aux.posicionsPortes;
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
            
        bool ocupada = PosicioOcupada(spawnPosition, posUsades, 1);
        int intentos = 0;

        while(ocupada && intentos < 100){ //comporbar que spawnPosition es buida
            x = Random.Range(-ampladaMapa + petitaAmplada, ampladaMapa - petitaAmplada);
            y = Random.Range(-alturaMapa + petitaAltura, alturaMapa - petitaAltura);
            spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);

            ocupada = PosicioOcupada(spawnPosition, posUsades, 1);
            intentos++;
        }

        posUsades.Add(spawnPosition);

        GameObject instanciarHabitacio = Instantiate(habitacioSpawn, spawnPosition, Quaternion.identity);
        habitacionsInstanciades.Add(instanciarHabitacio);

        GestioSpawn aux = instanciarHabitacio.GetComponent<GestioSpawn>();
            
        aux.TancarPortesAleatories();
        aux.PosicioPortes();
            
        List<Vector3> posPortes = aux.posicionsPortes;
        posPortesPerHabitacio.Add(posPortes);
    }


    void FerPassadis(){
        for (int i = 0; i < posPortesPerHabitacio.Count; i++){
            int habitacio = i;
            for (int j = 0; j < posPortesPerHabitacio[i].Count; j++){
                Vector3 porta1 = posPortesPerHabitacio[i][j];
                int[] indexPorta2 = TrobarPortaMesPropera(habitacio,porta1);
                Vector3 porta2 = new Vector3();
                if (indexPorta2[0] != -1 && indexPorta2[1] != -1){
                    porta2 = posPortesPerHabitacio[indexPorta2[0]][indexPorta2[1]];
                     if (porta1 != Vector3.zero && porta2 != Vector3.zero){
                        ConnectarPortes(porta1,porta2);
                        AfegirPortaUsada(habitacio, j);
                        AfegirPortaUsada(indexPorta2[0], indexPorta2[1]);
                    }
                }

               
            }
        }
    }

    int[] TrobarPortaMesPropera(int habitacio, Vector3 portaRef){
        int[] indexPortaSeleccionada = {-1, -1};
        float distanciaMinima = float.MaxValue;

        for (int i = 0; i < posPortesPerHabitacio.Count; i++){
            if (i != habitacio){
                List<Vector3> posPortesHabitacio = posPortesPerHabitacio[i];
                foreach (Vector3 porta in posPortesHabitacio){
                    int indexPorta = posPortesPerHabitacio[i].IndexOf(porta);
                    if (!portesUsades.ContainsKey(i) || (portesUsades.ContainsKey(i) && !portesUsades[i].Contains(indexPorta))){
                        float distancia = Vector3.Distance(portaRef, porta);
                        if (distancia < distanciaMinima){
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

    void ConnectarPortes(Vector3 porta1, Vector3 porta2){
        Vector3Int tilePos1 = tilemapMapa.WorldToCell(porta1);
        Vector3Int tilePos2 = tilemapMapa.WorldToCell(porta2);

        int startX = tilePos1.x;
        int startY = tilePos1.y;
        int endX = tilePos2.x;
        int endY = tilePos2.y;

        int directionX = (startX < endX) ? 1 : -1;
        int directionY = (startY < endY) ? 1 : -1;

        for (int x = startX; x != endX; x += directionX) {
            Vector3Int tilePos = new Vector3Int(x, startY, 0);
            if (!PosicioTileMapOcupada(tilePos) && !PosicioOcupada(new Vector3(x, startY, 0), posUsades, 0)) {
                ColocarPassadisHoritzontal(tilePos);
            }
        }

        for (int y = startY; y != endY; y += directionY) {
            Vector3Int tilePos = new Vector3Int(endX, y, 0);
            if (!PosicioTileMapOcupada(tilePos) && !PosicioOcupada(new Vector3(endX, y, 0), posUsades, 0)) {
                ColocarPassadisVertical(tilePos);
            }
        }    
    }
    
    void ColocarPassadisHoritzontal(Vector3Int tilePos){
        tilemapMapa.SetTile(tilePos, terraPassadis);

        /*Vector3Int paretInfPos = new Vector3Int(tilePos.x, tilePos.y - 1, 0);
        tilemapCollision.SetTile(paretInfPos, paretInf);
        tilemapMapa.SetTile(paretInfPos, terraPassadis);

        Vector3Int paretSupDPos = new Vector3Int(tilePos.x, tilePos.y + 1, 0);
        tilemapDecoracio.SetTile(paretSupDPos, paretSupD);
        tilemapMapa.SetTile(paretSupDPos, terraPassadis);

        Vector3Int paretSupPos = new Vector3Int(tilePos.x, tilePos.y + 2, 0);
        tilemapCollision.SetTile(paretSupPos, paretSup);*/
    }

    void ColocarPassadisVertical(Vector3Int tilePos){
        tilemapMapa.SetTile(tilePos, terraPassadis);

        /*Vector3Int paretEsqPos = new Vector3Int(tilePos.x -1 , tilePos.y, 0);
        tilemapCollision.SetTile(paretEsqPos, paretEsq);
        tilemapMapa.SetTile(paretEsqPos, terraPassadis);

        Vector3Int paretDrtPos = new Vector3Int(tilePos.x + 1, tilePos.y, 0);
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

        foreach (Vector3 usada in posUsades){
            float minX = usada.x - separacioHabitacions;
            float maxX = usada.x + granAmplada + separacioHabitacions;
            float minY = usada.y - separacioHabitacions;
            float maxY = usada.y + granAltura;

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
        TileBase tile = tilemapMapa.GetTile(pos);
        return tile != null;
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

        posUsades.Clear();
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
