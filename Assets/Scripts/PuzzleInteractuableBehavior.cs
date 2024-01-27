using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PuzzleInteractuableBehavior : MonoBehaviour
{
    //Les dos plaques que volem comprovar
    [SerializeField] PuzzleInteractable primeraPlaca;
    [SerializeField] PuzzleInteractable segonaPlaca;
    private bool completades = false;
    [SerializeField] AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void controlarPlaques()
    {
        if (primeraPlaca.EstaSiguentPitjada())
        {
            if (primeraPlaca.IsOFF()) { primeraPlaca.activarPlaca(); primeraPlaca.ReproduirSo(); }

            if (segonaPlaca.EstaSiguentPitjada())
            {
                segonaPlaca.activarPlaca();
                completades = true;
                Debug.Log("PLAQUES COMPLETADES");

            }
            else
            {
                segonaPlaca.desactivarPlaca();
            }



        }else if (segonaPlaca.EstaSiguentPitjada())
        {
            if (segonaPlaca.IsOFF()) { segonaPlaca.activarPlaca(); segonaPlaca.ReproduirSo(); }

            if (primeraPlaca.EstaSiguentPitjada())
            {
                primeraPlaca.activarPlaca();

                completades = true;
                Debug.Log("PLAQUES COMPLETADES");

            }
            else
            {
                primeraPlaca.desactivarPlaca();
            }
        }
    }


    public bool esCompleta()
    {
        return completades;
    }

}
