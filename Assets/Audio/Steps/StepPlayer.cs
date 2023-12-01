using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPlayer : MonoBehaviour
{

    [SerializeField]
    private AudioSource step1;
    [SerializeField]
    private AudioSource step2;
    [SerializeField]
    private AudioSource dash;
    private void playStep1()
    {
        step1.Play();
    }

    private void playStep2()
    {
        step2.Play();
    }

    private void plsyDash()
    {
        dash.Play();
    }

}
