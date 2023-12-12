using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private void AniamtionEnds(string name)
    {
        Debug.Log("Animation End: " + name);
        GetComponentInParent<PlayerController>().continueCombo(name);
    }
}
