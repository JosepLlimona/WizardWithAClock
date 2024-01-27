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

     private List<GameObject> portesAleatoriesTancades = new List<GameObject>();

    public void TancarPortesAleatories(List<Vector3[]> posicionsPortes){
        foreach (GameObject porta in posPortes){
            if (!PosicioEnLlista(porta.transform.position, posicionsPortes)){
                Vector3 posicio = porta.transform.position;
                string lloc = porta.name;
                
                GameObject paretPerColocar = ObtenirParetPerNom(lloc);

                if (paretPerColocar != null){
                    GameObject portaInstanciada = Instantiate(paretPerColocar, posicio, Quaternion.identity);
                    posPortesTancades.Add(posicio);
                    portesAleatoriesTancades.Add(portaInstanciada);
                    tancades++;
                } 
            }
        }
    }
    bool PosicioEnLlista(Vector3 posicio, List<Vector3[]> posicionsPortes){
        foreach(Vector3[] parella in posicionsPortes){
            if (posicio == parella[0] || posicio == parella[1]){
                return true;
            }
        }
        return false;
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

    public GameObject PortaAleatoria(){
        GameObject porta = posPortes[Random.Range(0,posPortes.Length)];
        return porta;
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
