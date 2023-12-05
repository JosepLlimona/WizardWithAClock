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

    public int anterior = 0;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap(){
        for (int x = 0; x < ampladaMapa; x++){
            for (int y = 0; y < alturaMapa; y++){
                Vector3 spawnPosition = new Vector3(x * (ampladaMapa + 5f) + Random.Range(-5f, 5f), y * (alturaMapa+ 5f) + Random.Range(-5f, 5f), 0);
                int nRandom = Random.Range(1,4);
                if (anterior == 3){ //habitacio anterior es gran
                    nRandom = Random.Range(1,3);
                }
                else if(anterior == 1){ //habitacio anterior es petita
                    nRandom = Random.Range(2,4);
                }

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

    void Update()
    {
        
    }
}
