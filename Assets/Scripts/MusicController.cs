using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public List<AudioSource> audios;

    // Start is called before the first frame update
    void Start()
    {
        audios = new List<AudioSource>();
        foreach(AudioSource audio in GameObject.FindObjectsOfType<AudioSource>())
        {
            audios.Add(audio);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            muteAudio();
        }
    }

    private void muteAudio()
    {
        foreach(AudioSource audio in audios)
        {
            audio.enabled = !audio.enabled;
        }
    }
}
