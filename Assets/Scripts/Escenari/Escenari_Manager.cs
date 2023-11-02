using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escenari_Manager : MonoBehavior {

    public Vector2 mida;
    public int numQuadrat;
    public Vector2 midaParets = new Vector2(2,3);

    [Range(0, 100)]
    public float midaRelativaParets = 10;

    public GameObject parets;
    public GameObject decoracio;
    public GameObject fpsController;

    int n;

    void Start(){
        int[,] escenari = Escenari.SpawnEscenari(mida, numQuadrat, 100/midaRelativaParets, midaParets);
        InstanciarEscenari(escenari);
    }

    void InstanciarEscenari(int[,] escenari){
        bool _i = false;

        for (int i = 0; i < escenari.GetLength(0); i++){
            for(int j = 0; j < escenari.GetLength(1); j++){
                if (escenari[i,j] == 0){
                    if(ComprovarPosicio(escenari, new Vector2(i,j))){
                        GameObject go = Instantiate(parets, new Vector3(i, 1.5f, j), Quaternion.identity) as GameObject;
                        go.transform.SetParent(transform);
                    }
                else{
                    if(!_i){
                        Instantiate(fpsController, new Vector3(i, 1, j), Quaternion.identity);
                        _i = true;
                    }
                    else{
                        int r = Random.Range(0,100);
                        if(r < 1){
                            Instantiate(decoracio, new Vector3(i, 1, j), Quaternion.identity);
                        }
                    }
                }
            
            }
        }
    }

    bool ComprovarPosicio(int[,] escenari, Vector2 posicio){
        if(posicio.x >= 1 && posicio.y >= 1 && posicio.y < escenari.GetLength(1) - 1 && posicio.x < escenari.GetLength(0) - 1){
            if(escenari[posicio.x + 1, posicio.y] != 0) return true;
            if(escenari[posicio.x - 1, posicio.y] != 0) return true;
            if(escenari[posicio.x, posicio.y+1] != 0) return true;
            if(escenari[posicio.x + 1, posicio.y-1] != 0) return true;
        }
        return false;
    }

}
