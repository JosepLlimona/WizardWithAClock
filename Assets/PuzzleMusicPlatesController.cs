using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMusicPlatesController : PuzzleController
{
    [SerializeField] private List<MusicalPlateController> musicalPlates = new List<MusicalPlateController>();
    [SerializeField] private int maximRondes;
    private List<int> ordrePlaquesMusicals; //s'anira resetejant cada vegada que el jugador s'equivoqui
    private bool jugadorEquivocat = false; //si sequivoca resetegem
    private bool isPlayerTurn = false; //per saber si li toca al jugador o no
    private int randomPlate;
    int placaActualHauriaSerPitjadaPerJugador = 0;
    int indexRonda = 1;
    int idPlacaActualPitjada = 0;
    bool playerHaPitjatPlaca = false; 
   bool revisarPlaques = true;
    bool handlingPlayerTurn = false;
    [SerializeField] private AudioSource ticSound;
    // Start is called before the first frame update
    void Start()
    {
        ordrePlaquesMusicals = new List<int>(maximRondes);
        GetNewMusicalSequence();
        
    }

    private void OnEnable()
    {

        Debug.Log("holaaa");
            MusicalPlateController.OnMusicalPlatePressed += HandlePlayerTurn;
        
    }

    private void OnDisable()
    {
   
        
            MusicalPlateController.OnMusicalPlatePressed -= HandlePlayerTurn;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startPlate.IsON() && !puzzleStarted)
        {

            puzzleStarted = true;
            OnMusicalPuzzleStart();
        }


        if (!puzzleStarted)
        {
            for (int i = 0; i < musicalPlates.Count; i++)
            {
                if (musicalPlates[i].EstaSiguentPitjada())
                {
                    //playerHaPitjatPlaca = true;
                    //idPlacaActualPitjada = musicalPlates[i].getIdPlaca();
                    ferSonarPlaca(musicalPlates[i]);

                    Debug.Log("Abans de començar el puzzle id placa: " + musicalPlates[i].getIdPlaca());
                    // revisarPlaques = false; //ja no fa falta revisar plaques

                }
            }
        }


      

    }

    private void OnMusicalPuzzleStart()
    {
        Debug.Log("OnMusicStart -> Abans On Pu<zzleStart ");
        OnPuzzleStart();
        Debug.Log("OnMusicStart -> Despres On Pu<zzleStart ");
        HandleTurns();
        // 3 tic i començar amb la primera placa
    }

    private bool encaraQuedenPlaquesPerPitjar()
    {
        return placaActualHauriaSerPitjadaPerJugador < ordrePlaquesMusicals.Count;
    }

    private void GetNewMusicalSequence()
    {
        Debug.Log("Getting New Sequence");
        int id;
        ordrePlaquesMusicals.Clear();
        //StartCoroutine(ReproducirSecuencia());
        for (int i = 0; i<maximRondes; i++)
        {
            id = GetRandomId();
            ordrePlaquesMusicals.Add(id);
            
            //Debug.Log(ordrePlaquesMusicals[i]);
        }
        
    }
    private void ferSonarPlaca(MusicalPlateController placa)
    {
        placa.activarPlaca();
        placa.ReproduirSo();
        placa.desactivarPlaca();
    }
    private int GetRandomId() //retorna una id de les plaques musicals aleatoria
    {
        return Random.Range(0, musicalPlates.Count);
    }


    private void HandleIATurn()
    {
        //cada vegada que sigui el torn de la ia he de fer sona totes les plaques fins el index de ronda
        StartCoroutine(ReproducirSecuencia());
        isPlayerTurn = true;
        revisarPlaques = true;
    }

    public void HandlePlayerTurn(int idPlacaActualPitjada)
    {
        if (isPlayerTurn)
        {
            handlingPlayerTurn = true;
            Debug.Log("Provaaa" + idPlacaActualPitjada);
            placaActualHauriaSerPitjadaPerJugador++;

            if(placaActualHauriaSerPitjadaPerJugador == indexRonda) { 
                indexRonda++;
                placaActualHauriaSerPitjadaPerJugador = 0;  
                isPlayerTurn = false; 
                if(indexRonda == maximRondes+1) { OnPuzzleEnd(); }
                else { HandleTurns(); }
                
            }
            
            
          
        }
        
        /*
         * isPlayerTurn = false;
        Debug.Log("HANDLING PLAYER TURN");
        playerHaPitjatPlaca = false;
        Debug.Log("El player ha pitjar una placa i comprovem si es igual" + idPlacaActualPitjada + ordrePlaquesMusicals[placaActualHauriaSerPitjadaPerJugador]);


        if (idPlacaActualPitjada == ordrePlaquesMusicals[placaActualHauriaSerPitjadaPerJugador])
        {
            placaActualHauriaSerPitjadaPerJugador++;
            Debug.Log("Sumem la placa acutal que estem mrian de la sequencia:" + placaActualHauriaSerPitjadaPerJugador);
            if (placaActualHauriaSerPitjadaPerJugador == indexRonda)
            {
                Debug.Log("hem passat de ronda");
                if (indexRonda == maximRondes)
                {
                    //hem guanyat el puzzle
                    isPlayerTurn = false;
                    OnPuzzleEnd();
                }
                else
                {
                    //encara queden rondes
                    placaActualHauriaSerPitjadaPerJugador = 0;
                    indexRonda++;
                    isPlayerTurn = false;
                    revisarPlaques = true;
                    //StartCoroutine(ReproducirSecuencia());
                    HandleTurns();
                }

            }
            else
            {

                revisarPlaques = true;
            }
        }
        else
        { //jugador ha fallat
            Debug.Log("el jugador s'ha ewquivcoat");
            indexRonda = 1;
            placaActualHauriaSerPitjadaPerJugador = 0;
            GetNewMusicalSequence();
            isPlayerTurn = false;
            revisarPlaques = true;
            HandleTurns();

        }
         */

    }


    private void HandleTurns()
    {

        //StartCoroutine(ReporduirTicsPrincipi());
        //StartCoroutine(esperar());
        
        if (!isPlayerTurn)
        { //el torn de la maquina
            Debug.Log("HANDLING IA TURN");
            HandleIATurn();
        }
        Debug.Log("Hanle turns -> despres del if ");
  
        
    }

    IEnumerator ReproducirSecuencia()
    {

        yield return new WaitForSeconds(3); // Ajusta el tiempo entre cada placa en la secuencia.
        for (int i = 0; i < indexRonda; i++)
        {
            yield return new WaitForSeconds(0.7f); // Ajusta el tiempo entre cada placa en la secuencia.
            int idPlacaAcutal = ordrePlaquesMusicals[i];
            Debug.Log("IndexRonda" + indexRonda);
            Debug.Log(musicalPlates[idPlacaAcutal].getIdPlaca());
            ferSonarPlaca(musicalPlates[idPlacaAcutal]);

        }

   

    }

    IEnumerator ReporduirTicsPrincipi()
    {
        yield return new WaitForSeconds(1f); // Ajusta el tiempo entre cada placa en la secuencia.
        for (int i = 0; i<3; i++)
        {
            ticSound.Play();
            yield return new WaitForSeconds(0.3f); // Ajusta el tiempo entre cada placa en la secuencia.
        }
        StartCoroutine(esperar());
    }

    IEnumerator esperar()
    {
        yield return new WaitForSeconds(2f); // Ajusta el tiempo entre cada placa en la secuencia.

    }


}

/*
 * 
 *if (startPlate.IsON() && !puzzleStarted && !puzzleComplet)
        {
            OnPuzzleStart();
        }
            //Primer agafem una placa aleatoria
            if (musicalPlates.Count > 0) {

            if (!isPlayerTurn) //IA TURN
            {
                if (ordrePlaquesMusicals.Count > 0) //Si hi han plaques guardades Hem d'activar altres plaques abans d'activar la nova
                {
                    for (int i = 0; i < ordrePlaquesMusicals.Count; i++)
                    {
                        musicalPlates[i].activarPlaca();
                        musicalPlates[i].desactivarPlaca();
                    }
                }
                //ara que ja hem recorregut totes les plaques, agafem una de nova
                randomPlate = Random.Range(0, musicalPlates.Count);
                musicalPlates[randomPlate].activarPlaca();
                musicalPlates[randomPlate].desactivarPlaca();
                //ara que ha sonat la placa, em guardo la seva id
                ordrePlaquesMusicals.Add(musicalPlates[randomPlate].getIdPlaca());
                isPlayerTurn = true;

            }
            else //isPlayerTurn
            {
                //si es el torn del jugador hem de mirar per ordre si pitja be les plaques
                
                for(int i = 0; i< musicalPlates.Count; i++)
                {
                    if (encaraQuedenPlaquesPerPitjar())
                    {
                        if (musicalPlates[i].EstaSiguentPitjada())
                        {
                            if (musicalPlates[i].getIdPlaca() == ordrePlaquesMusicals[placaActualHauriaSerPitjadaPerJugador])
                            {//estem pitjant la placa correcte
                                placaActualHauriaSerPitjadaPerJugador++; //passem a la seguent
                            }
                            else //ens hem equivocat i hem de reiniciar
                            {
                                placaActualHauriaSerPitjadaPerJugador = 0;
                                ordrePlaquesMusicals.Clear(); //resetejem l'ordre
                                isPlayerTurn = false;
                            }

                        }
                    }
                    else
                    {
                        isPlayerTurn = false;
                    }

                }
            }

        } 
 */