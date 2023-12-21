using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{

    [SerializeField] protected UIController ui;


    [SerializeField] protected PlateController startPlate;

    [SerializeField] protected Chest chest;
    [SerializeField] protected AudioSource puzzleCompletedAudio, puzzleStartedAudio;
    protected bool puzzleStarted = false;

    protected bool puzzleComplet = false;
    // Start is called before the first frame update

    protected void OnPuzzleStart()
    {
        soundPuzzlestarted();
        puzzleStarted = true;
        ui.setPuzzleStarted();
        startPlate.FadeOutPlaca();
        Destroy(startPlate);
        //startPlate.gameObject.SetActive(false);


    }
    protected void OnPuzzleEnd()
    {
        soundPuzzleCompleted();
        chest.canOpenChest();
        puzzleComplet = true;
        //AQUI HAURE DE FER SPAWN DEL COFRE
        Debug.Log("PuzzleCompletat");
        chest.EnableChest();
        Destroy(startPlate);
        //Fer desapareixer al mirror player
 
        //reproduir so del cofre
    }

    public void soundPuzzleCompleted()
    {
        if (puzzleCompletedAudio != null)
        {
            puzzleCompletedAudio.Play();
        }
    }
    public void soundPuzzlestarted()
    {
        if (puzzleStartedAudio != null)
        {
            puzzleStartedAudio.Play();
        }
    }
}
