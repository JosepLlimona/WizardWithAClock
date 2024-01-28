using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private List<AudioSource> audios;
    public bool isMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        audios = new List<AudioSource>();
        getAllAudio();

    }

    private void getAllAudio()
    {
        foreach (AudioSource audio in GameObject.FindObjectsOfType<AudioSource>())
        {
            audios.Add(audio);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && isMuted){
            unmuteAll();
        }
        else if(Input.GetKeyDown(KeyCode.M) && !isMuted)
        {
            muteAll();
        }
    }

    public void muteAudio()
    {
        getAllAudio();
        if (isMuted)
        {
            muteAll();
        }
        else
        {
            unmuteAll();
        }
    }

    private void muteAll()
    {
        foreach (AudioSource audio in audios)
        {
            if (audio != null)
            {
                audio.enabled = false;
            }
        }
        isMuted = true;
    }

    private void unmuteAll()
    {
        foreach (AudioSource audio in audios)
        {
            if (audio != null)
            {
                audio.enabled = true;
            }
        }
        isMuted = false;
    }
}
