using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public GameObject Player;
    public Tilemap tilemapMapa;
    public Tilemap tilemapDecoracio;
    public Tilemap tilemapCollision;
    public Tile terraPassadis;
    public Tile paretEsq;
    public Tile paretDrt;
    public Tile paretInf;
    public Tile paretSup;
    public Tile paretSupD;

    public GameObject[] habPetita;
    private List<int> petitaUsades = new List<int>();
    public GameObject[] habMitjana;
    private List<int> mitjanaUsades = new List<int>();
    public GameObject[] habGran;
    private List<int> granUsades = new List<int>();
    public GameObject habBoss;
    public GameObject habitacioSpawn;
    public GameObject habitacioLobby;

    public GameObject[] habPuzzle;

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

    private List<Vector3[]> parellesPortes = new List<Vector3[]>(); //vector3[2] indica el tipus: si (0,0,0) = horitzontal, si (1,1,1) = vertical;

    private List<GameObject> habitacionsInstanciades = new List<GameObject>();

    public int nivellActual = 1; //Només genera Boss si es al 2 o al 4


    void Start()
    {
        AnarAlLobby();
    }

    public void AnarAlLobby(){
        
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        GameObject lobby = Instantiate(habitacioLobby, spawnPosition, Quaternion.identity);
        habitacionsInstanciades.Add(lobby);
        GestioHabitacio gestioL = lobby.GetComponent<GestioHabitacio>();
        gestioL.MostrarText();

        PlayerController gestioP = Player.GetComponent<PlayerController>();
        gestioP.setPosition(new Vector3(1.2f, 0.64f, 0));
    }

    public void GenerateMap(){

        nivellActual++;
        Debug.Log(nivellActual);
        float x = Random.Range(-ampladaMapa + petitaAmplada, ampladaMapa - petitaAmplada);
        float y = Random.Range(-alturaMapa + petitaAltura, alturaMapa - petitaAltura);
        Vector3 spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);
        GameObject spawn = Instantiate(habitacioSpawn, spawnPosition, Quaternion.identity);
        GestioSpawn gestioS = spawn.GetComponent<GestioSpawn>();
        GameObject posSpawnPlayer = gestioS.posSpawn;

        PlayerController gestioP = Player.GetComponent<PlayerController>();
        gestioP.setPosition(posSpawnPlayer.transform.position);

        habitacionsInstanciades.Add(spawn);
        
        for (int i = 0; i < habitacionsMapa; i++){
            int tipus;
            float ampladaHabitacio;
            float alturaHabitacio;

            ObtenirTipusHabitacio(out tipus, out ampladaHabitacio, out alturaHabitacio);

            bool ocupada = true;
            int intents = 0;
            Vector3[] parella = new Vector3[3];
            string tipusPorta = "";

            while (ocupada && intents < 1000){

                GameObject habitacioExistent = habitacionsInstanciades[i];
                GameObject portaOrigen;
                if (i == 0){
                    gestioS = habitacioExistent.GetComponent<GestioSpawn>();
                    portaOrigen = gestioS.PortaAleatoria();
                }
                else{ 
                    GestioHabitacio gestio = habitacioExistent.GetComponent<GestioHabitacio>();
                    portaOrigen = gestio.PortaAleatoria();
                }
                
                if (portaOrigen.tag == "PortaEsq"){
                    x = Random.Range(habitacioExistent.transform.position.x - 10f, habitacioExistent.transform.position.x - 15f);
                    y = PosicioHabitacioNovaY(portaOrigen, tipus);
                    tipusPorta = "PortaDreta";
                    parella[2] = new Vector3(0,0,0);
                }
                else if (portaOrigen.tag == "PortaDreta"){
                    x = Random.Range(habitacioExistent.transform.position.x + 10f, habitacioExistent.transform.position.x + 15f);
                    y = PosicioHabitacioNovaY(portaOrigen, tipus);
                    tipusPorta = "PortaEsq";
                    parella[2] = new Vector3(0,0,0);
                }
                else if (portaOrigen.tag == "PortaSuperior"){
                    x = PosicioHabitacioNovaX(portaOrigen, tipus);;
                    y = Random.Range(habitacioExistent.transform.position.y + 10f, habitacioExistent.transform.position.y + 15f);
                    tipusPorta = "PortaInferior";
                    parella[2] = new Vector3(1,1,1);
                }
                else if (portaOrigen.tag == "PortaInferior"){
                    x = PosicioHabitacioNovaX(portaOrigen, tipus);;
                    y = Random.Range(habitacioExistent.transform.position.y - 10f, habitacioExistent.transform.position.y - 15f);
                    tipusPorta = "PortaSuperior";
                    parella[2] = new Vector3(1,1,1);
                }

                parella[0] = portaOrigen.transform.position;

                spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);

                ocupada = PosicioOcupada(spawnPosition, habitacionsInstanciades, tipus);
            }

            if (!ocupada){
                anterior = tipus;

                GameObject habitacioRandom = ObtenirHabitacioRandom(tipus);
                GameObject habitacioNova = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);

                habitacionsInstanciades.Add(habitacioNova);

                habitacioNova.name = i.ToString();

                GestioHabitacio aux = habitacioNova.GetComponent<GestioHabitacio>();

                parella[1] = aux.PosicioPortaPerTipus(tipusPorta);

                parellesPortes.Add(parella);
            }
        }
        CrearHabitacioBoss();

        if(habPuzzle.Length != 0){
            CrearHabitacioPuzzle();
        }

        GestioSpawn auxS = habitacionsInstanciades[0].GetComponent<GestioSpawn>();
        auxS.TancarPortesAleatories(parellesPortes);

        for (int i = 1; i < habitacionsInstanciades.Count; i++){
            GestioHabitacio aux = habitacionsInstanciades[i].GetComponent<GestioHabitacio>();
            aux.TancarPortesAleatories(parellesPortes);
        }

        FerPassadis();
    }

    void CrearHabitacioBoss(){
        bool ocupada = true;
        Vector3[] parella = new Vector3[3];
        string tipusPorta = "";
        int i = 1;
        Vector3 spawnPosition = new Vector3();

        while (ocupada && i < habitacionsInstanciades.Count){
            GameObject habitacioExistent = habitacionsInstanciades[i];
            GestioHabitacio gestio = habitacioExistent.GetComponent<GestioHabitacio>();
            GameObject portaOrigen = gestio.PortaPerTipus("PortaSuperior");

            if (portaOrigen != null){
                parella[0] = portaOrigen.transform.position;

                float x = PosicioHabitacioNovaX(portaOrigen, 3);
                float y = Random.Range(habitacioExistent.transform.position.y + 10f, habitacioExistent.transform.position.y + 15f);
                tipusPorta = "PortaInferior";
                parella[2] = new Vector3(1,1,1);

                spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);

                ocupada = PosicioOcupada(spawnPosition, habitacionsInstanciades, 3);
                i++;
            }
        }
        if (!ocupada){
            GameObject Boss = Instantiate(habBoss, spawnPosition, Quaternion.identity);
            habitacionsInstanciades.Add(Boss);
            GestioHabitacio aux = Boss.GetComponent<GestioHabitacio>();

            parella[1] = aux.PosicioPortaPerTipus(tipusPorta);
            parellesPortes.Add(parella);
        }
    }

    void CrearHabitacioPuzzle(){
        bool ocupada = true;
        Vector3[] parella = new Vector3[3];
        string tipusPorta = "";
        int i = 1;
        Vector3 spawnPosition = new Vector3();

        while (ocupada && i < habitacionsInstanciades.Count - 1){
            GameObject habitacioExistent = habitacionsInstanciades[i];
            GestioHabitacio gestio = habitacioExistent.GetComponent<GestioHabitacio>();
            GameObject portaOrigen = gestio.PortaPerTipus("PortaDreta");

            if (portaOrigen != null){
                Debug.Log("hay puerta origen");
                parella[0] = portaOrigen.transform.position;

                float x = Random.Range(habitacioExistent.transform.position.x + 10f, habitacioExistent.transform.position.x + 15f);
                float y = PosicioHabitacioNovaY(portaOrigen,2);
                tipusPorta = "PortaEsq";
                parella[2] = new Vector3(0,0,0);

                spawnPosition = new Vector3(Mathf.Round(x / tileSize) * tileSize, Mathf.Round(y / tileSize) * tileSize, 0);

                ocupada = PosicioOcupada(spawnPosition, habitacionsInstanciades, 2);
                i++;
            }
        }
        if (!ocupada){
            GameObject puzzle = Instantiate(habPuzzle[Random.Range(0, habPuzzle.Length)], spawnPosition, Quaternion.identity);
            habitacionsInstanciades.Add(puzzle);
            GestioHabitacio aux = puzzle.GetComponent<GestioHabitacio>();

            parella[1] = aux.PosicioPortaPerTipus(tipusPorta);
            parellesPortes.Add(parella);

            puzzle.name = "Puzzle";
        }
    }
    
    float PosicioHabitacioNovaX(GameObject portaRef, int tipus){
        float x;
        if (tipus == 1){
            x = portaRef.transform.position.x - 1.6f;
        }
        else if (tipus == 2){
            x = portaRef.transform.position.x - 2.56f;
        }
        else{
            x = portaRef.transform.position.x - 3.52f;
        }
        return x;
    }

    float PosicioHabitacioNovaY(GameObject portaRef, int tipus){
        float y;
        if (tipus == 1){
            y = portaRef.transform.position.y - 1.6f;
        }
        else if (tipus == 2){
            y = portaRef.transform.position.y - 2.56f;
        }
        else{
            y = portaRef.transform.position.y - 3.84f;
        }
        return y;
    }

    void FerPassadis(){
        foreach (Vector3[] parella in parellesPortes){
            ConnectarPortes(parella[0], parella[1], parella[2]);
        }
    }

    void ConnectarPortes(Vector3 porta1, Vector3 porta2, Vector3 tipus){
        Vector3Int tilePos1 = tilemapMapa.WorldToCell(porta1);
        Vector3Int tilePos2 = tilemapMapa.WorldToCell(porta2);

        int startX = tilePos1.x;
        int startY = tilePos1.y;
        int endX = tilePos2.x;
        int endY = tilePos2.y;

        int directionX = (startX < endX) ? 1 : -1;
        int directionY = (startY < endY) ? 1 : -1;

        if (tipus == new Vector3(0,0,0)){
            startX +=directionX;
            for (int x = startX; x != endX; x += directionX) {
                Vector3Int tilePos = new Vector3Int(x, startY, 0);
                if (true) {
                    ColocarPassadisHoritzontal(tilePos);
                }
            }
        }

        else{
            startY += directionY;
            for (int y = startY; y != endY; y += directionY) {
                Vector3Int tilePos = new Vector3Int(endX, y, 0);
                if (true) {
                    ColocarPassadisVertical(tilePos);
                }
            }
        }
          
    }
    
    void ColocarPassadisHoritzontal(Vector3Int tilePos){
        tilemapMapa.SetTile(tilePos, terraPassadis);
        tilemapCollision.SetTile(tilePos, paretInf);

        Vector3Int paretInfPos = new Vector3Int(tilePos.x, tilePos.y + 1, 0);
        tilemapMapa.SetTile(paretInfPos, terraPassadis);

        Vector3Int paretSupDPos = new Vector3Int(tilePos.x, tilePos.y + 2, 0);
        tilemapDecoracio.SetTile(paretSupDPos, paretSupD);
        tilemapMapa.SetTile(paretSupDPos, terraPassadis);

        Vector3Int paretSupPos = new Vector3Int(tilePos.x, tilePos.y + 3, 0);
        tilemapCollision.SetTile(paretSupPos, paretSup);
    }

    void ColocarPassadisVertical(Vector3Int tilePos){
        tilemapMapa.SetTile(tilePos, terraPassadis);
        tilemapCollision.SetTile(tilePos, paretEsq);

        Vector3Int paretEsqPos = new Vector3Int(tilePos.x + 1 , tilePos.y, 0);
        
        tilemapMapa.SetTile(paretEsqPos, terraPassadis);

        Vector3Int paretDrtPos = new Vector3Int(tilePos.x + 2, tilePos.y, 0);
        tilemapCollision.SetTile(paretDrtPos, paretDrt);
        tilemapMapa.SetTile(paretDrtPos, terraPassadis);
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

    void RespawnHabitacioBoss(){
        GestioHabitacio habitacioBoss = habitacionsInstanciades[habitacionsInstanciades.Count-1].GetComponent<GestioHabitacio>();
        if (habitacioBoss != null){
            habitacioBoss.ClearHabitacio();
            habitacionsInstanciades.RemoveAt(habitacionsInstanciades.Count-1);
            parellesPortes.RemoveAt(parellesPortes.Count-1);
            Destroy(habitacionsInstanciades[habitacionsInstanciades.Count-1]);

            CrearHabitacioBoss();
        }

    }

    public void ClearMap(){

        tilemapMapa.ClearAllTiles();
        tilemapDecoracio.ClearAllTiles();
        tilemapCollision.ClearAllTiles();

        GestioSpawn auxS = habitacionsInstanciades[0].GetComponent<GestioSpawn>();
        if (auxS != null){
            auxS.ClearSpawn();
            Destroy(habitacionsInstanciades[0]);
        }

        GestioHabitacio aux = habitacionsInstanciades[0].GetComponent<GestioHabitacio>();
        if (aux != null){
            aux.ClearHabitacio();
            Destroy(habitacionsInstanciades[0]);
        }

        for (int i = 1; i < habitacionsInstanciades.Count; i++){
            aux = habitacionsInstanciades[i].GetComponent<GestioHabitacio>();
            if (aux != null){
                aux.ClearHabitacio();
                Destroy(habitacionsInstanciades[i]);
            }
            
        }

        petitaUsades.Clear();
        mitjanaUsades.Clear();
        granUsades.Clear();
        habitacionsInstanciades.Clear();
        parellesPortes.Clear();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            ClearMap();
            if (nivellActual >= 4){
                AnarAlLobby();
                nivellActual = 1;
            }
            else{
                GenerateMap();
            }
        }
    }

}
