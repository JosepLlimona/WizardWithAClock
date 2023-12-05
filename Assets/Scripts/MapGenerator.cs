using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public GameObject[] habPetita;
    public GameObject[] habMitjana;
    public GameObject[] habGran;

    public int alturaMapa = 10;
    public int ampladaMapa = 10;

    public float petitaAmplada = 13f;
    public float petitaAltura = 13f;
    public float mitjanaAmplada = 19f;
    public float mitjanaAltura = 19f;
    public float granAmplada = 25f;
    public float granAltura = 27f;

    public float separacioHabitacions = 5f;
    public int anterior = 0;
    private bool[,] quadriculaPos; 

    void Start()
    {
        quadriculaPos = new bool[ampladaMapa,alturaMapa];
        GenerateMap();
    }

    void GenerateMap(){

        List<Vector3> posUsades = new List<Vector3>();
        for (int x = 0; x < ampladaMapa; x++){
            for (int y = 0; y < alturaMapa; y++){
                Vector3 spawnPosition = new Vector3(x * (ampladaMapa + separacioHabitacions) + Random.Range(-separacioHabitacions, separacioHabitacions), y * (petitaAltura + separacioHabitacions) + Random.Range(-separacioHabitacions, separacioHabitacions),0);
                
                int nRandom = Random.Range(1,4);
                if (anterior == 3){ //habitacio anterior es gran
                    nRandom = Random.Range(1,3);
                }
                else if(anterior == 1){ //habitacio anterior es petita
                    nRandom = Random.Range(2,4);
                }
    
                while(PosicioOcupada(spawnPosition, posUsades, nRandom) || QuadriculaOcupada(x,y)){ //comporbar que spawnPosition es buida
                    spawnPosition = new Vector3(x * (ampladaMapa + separacioHabitacions) + Random.Range(-separacioHabitacions, separacioHabitacions), y * (alturaMapa+ separacioHabitacions) + Random.Range(-separacioHabitacions, separacioHabitacions),0);
                    x = Mathf.FloorToInt(spawnPosition.x / 1f);
                    y = Mathf.FloorToInt(spawnPosition.y / 1f);
                }

                posUsades.Add(spawnPosition);
                MarcarQuadricula(x,y);

                if (nRandom ==1){ //instanciar habitacio petita
                    GameObject habitacioRandom = habPetita[Random.Range(0, habPetita.Length)];
                    GameObject instaciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);
                }
                else if(nRandom == 2){ //instanciar habitacio mitjana
                    GameObject habitacioRandom = habMitjana[Random.Range(0, habMitjana.Length)];
                    GameObject instaciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);
                }
                else{ //instanciar habitacio gran
                    GameObject habitacioRandom = habGran[Random.Range(0, habGran.Length)];
                    GameObject instaciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);
                }
            
             }

        }
    }

    bool QuadriculaOcupada(int x, int y){
        return quadriculaPos[x,y];
    }

    void MarcarQuadricula(int x, int y){
        quadriculaPos[x,y] = true;
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
            if (Vector3.Distance(pos, usada) < amplada + separacioHabitacions){
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        
    }
}
