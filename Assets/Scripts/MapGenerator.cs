using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

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


    void Start()
    {
        GenerateMap();
    }

    void GenerateMap(){

        List<Vector3> posUsades = new List<Vector3>();
        for (int i = 0; i < habitacionsMapa; i++){
                
            int tipus; 
            float ampladaHabitacio;
            float alturaHabitacio;

            ObtenirTipusHabitacio(out tipus, out ampladaHabitacio, out alturaHabitacio);

            float x = Random.Range(-ampladaMapa + ampladaHabitacio, ampladaMapa - ampladaHabitacio);
            float y = Random.Range(-alturaMapa + alturaHabitacio, alturaMapa - alturaHabitacio);

            Vector3 spawnPosition = new Vector3(x,y,0);
            
            bool ocupada = PosicioOcupada(spawnPosition, posUsades, tipus);
            int intentos = 0;

            while(ocupada && intentos < 100){ //comporbar que spawnPosition es buida
                x = Random.Range(-ampladaMapa + ampladaHabitacio, ampladaMapa - ampladaHabitacio);
                y = Random.Range(-alturaMapa + alturaHabitacio, alturaMapa - alturaHabitacio);
                spawnPosition = new Vector3(x,y,0);

                ocupada = PosicioOcupada(spawnPosition, posUsades, tipus);
                intentos++;
            }

            posUsades.Add(spawnPosition);
            anterior = tipus;

            GameObject habitacioRandom = ObtenirHabitacioRandom(tipus);
            GameObject instaciarHabitacio = Instantiate(habitacioRandom, spawnPosition, Quaternion.identity);
        }
        
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
            if (Vector3.Distance(pos, usada) < amplada + separacioHabitacions){
                return true;
            }
        }
        return false;
    }
}