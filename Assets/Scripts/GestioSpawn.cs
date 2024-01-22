using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestioSpawn : MonoBehaviour{
    public GameObject posSpawn;
    public GameObject[] posPortes;
    public GameObject tileParetEsq;
    public GameObject tileParetDreta;
    public GameObject tileParetSuperior;
    public GameObject tileParetInferior;

    private int tancades = 0;
    private List<Vector3> posPortesTancades = new List<Vector3>();
    private bool portesTancades = false;
    public List<Vector3> posicionsPortes = new List<Vector3>();

     private List<GameObject> portesAleatoriesTancades = new List<GameObject>();

    public void TancarPortesAleatories(){
        int i = 0;
        while (i < posPortes.Length && tancades < posPortes.Length / 2){
            float nRandom = Random.Range(0f,1f);

            if (nRandom < 0.5f){
                Vector3 posicio = posPortes[i].transform.position;
                string lloc = posPortes[i].name;
                
                GameObject paretPerColocar = ObtenirParetPerNom(lloc);

                if (paretPerColocar != null){
                    GameObject portaInstanciada = Instantiate(paretPerColocar, posicio, Quaternion.identity);
                    posPortesTancades.Add(posicio);
                    portesAleatoriesTancades.Add(portaInstanciada);
                    tancades++;
                } 
            }
            i++;
        }
        PosicioPortes();
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
    public void PosicioPortes(){
        List<Vector3> portes = new List<Vector3>();

        for (int i = 0; i < posPortes.Length; i++){
            GameObject porta = posPortes[i];
            Vector3 posicio = porta.transform.position;
            if (!posPortesTancades.Contains(posicio)){  
                portes.Add(posicio);
                porta.name = (i).ToString();

            }
        } 
        posicionsPortes = portes;
    }

    public void ClearSpawn(){
        foreach (GameObject porta in portesAleatoriesTancades){
            Destroy(porta);
        }
        posPortesTancades.Clear();
        portesAleatoriesTancades.Clear();
        portesTancades = false;
    }
}
