using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public GameObject[] habPetita;
    public GameObject[] habMitjana;
    public GameObject[] habGran;

    public int habitacionsMapa = 5;
    public float alturaMapa = 100f;
    public float ampladaMapa = 100f;

    public float petitaAmplada = 13f;
    public float petitaAltura = 13f;
    public float mitjanaAmplada = 19f;
    public float mitjanaAltura = 19f;
    public float granAmplada = 25f;
    public float granAltura = 27f;

    public float separacioHabitacions = 1f;
    public int anterior = 0; 

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap(){

        List<Vector3> posUsades = new List<Vector3>();
        for (int i = 0; i < habitacionsMapa; i++){
                
            int nRandom = Random.Range(1,4);
            if (anterior == 3){ //habitacio anterior es gran
                nRandom = Random.Range(1,3);
            }
            else if(anterior == 1){ //habitacio anterior es petita
                nRandom = Random.Range(2,4);
            }

            float ampladaHabitacio;
            float alturaHabitacio;

            if (nRandom == 1){
                ampladaHabitacio = petitaAmplada;
                alturaHabitacio = petitaAltura;
            }
            else if (nRandom == 2){
                ampladaHabitacio = mitjanaAmplada;
                alturaHabitacio = mitjanaAltura;
            }
            else{
                ampladaHabitacio = granAmplada;
                alturaHabitacio = granAltura;
            }

            Debug.Log("se escoge tipo habitacion");

            float x = Random.Range(ampladaHabitacio, ampladaMapa - ampladaHabitacio);
            float y = Random.Range(alturaHabitacio, alturaMapa - alturaHabitacio);

            Vector3 spawnPosition = new Vector3(x,y,0);
            Debug.Log("punto de spawn");
            bool ocupada = PosicioOcupada(spawnPosition, posUsades, nRandom);
            int intentos = 0;

            while(ocupada && intentos < 100){ //comporbar que spawnPosition es buida
                x = Random.Range(ampladaHabitacio, ampladaMapa - ampladaHabitacio);
                y = Random.Range(alturaHabitacio, alturaMapa - alturaHabitacio);
                spawnPosition = new Vector3(x,y,0);

                ocupada = PosicioOcupada(spawnPosition, posUsades, nRandom);
                intentos++;
            }

            Debug.Log("punto de spawn correcto");
            posUsades.Add(spawnPosition);
            anterior = nRandom;

            if (nRandom ==1){ //instanciar habitacio petita
                GameObject habitacioRandom = habPetita[Random.Range(0, habPetita.Length)];
                GameObject instaciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);
                }
            else if(nRandom == 2){ //instanciar habitacio mitjana
                GameObject habitacioRandom = habMitjana[Random.Range(0, habMitjana.Length)];
                GameObject instaciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);                }
            else{ //instanciar habitacio gran
                GameObject habitacioRandom = habGran[Random.Range(0, habGran.Length)];
                GameObject instaciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);
            }
            Debug.Log("instanciada");
        }
        
    }

    bool PosicioOcupada(Vector3 pos, List<Vector3> posUsades, int tipus){
        float amplada;
        float altura; 
        Debug.Log("entra en posicion Ocupada");
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
                Debug.Log("posicion ocupada");
                return true;
            }
        }
        Debug.Log("posicion libre");
        return false;
    }

    void Update()
    {
        
    }
}
