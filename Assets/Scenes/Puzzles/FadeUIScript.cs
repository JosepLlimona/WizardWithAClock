using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
public class FadeUIScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private CanvasGroup pressText;
    [SerializeField] private bool esTextPress;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    public void ShowUI()
    {
        Debug.Log("Ara mostro les instruccions");
        myUIGroup.alpha = 0;
        if(pressText != null) pressText.gameObject.SetActive(false);
       fadeIn =  true;
        fadeOut = false;
        StartCoroutine(esperarFade());
    }

    public void HideUI()
    {
        //myUIGroup.alpha = 1;
        if (pressText != null) pressText.gameObject.SetActive(true);
        fadeOut = true;
        StartCoroutine(esperarFade());
        fadeIn = false;
    }

    public void ShowPressTextUI()
    {
        Debug.Log("Ara mostro el text press");
        myUIGroup.alpha = 0;
        fadeIn = true;
        fadeOut = false;
        StartCoroutine(esperarFade());
    }

    public void HidePressTextUI()
    {
        //myUIGroup.alpha = 1;
        fadeOut = true;
        StartCoroutine(esperarFade());
        fadeIn = false;
    }

    private void OnEnable()
    {
        if (!esTextPress)
        {

            CanvasController.onShowPuzzle += ShowUI;
            CanvasController.onHidePuzzle += HideUI;
        }
        UIController.OnPressShowText += ShowPressTextUI;
        UIController.OnPressDisableText += HidePressTextUI;
    }

    private void OnDisable()
    {
        if (!esTextPress)
        {

            CanvasController.onShowPuzzle -= ShowUI;
            CanvasController.onHidePuzzle -= HideUI;
        }
        UIController.OnPressShowText -= ShowPressTextUI;
        UIController.OnPressDisableText -= HidePressTextUI;
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        if (fadeIn)
        {
            //if (esTextPress) { myUIGroup.alpha = 0; }
                if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime*0.9f;
                if(myUIGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }


        if (fadeOut)
        {
            if (myUIGroup.alpha > 0)
            {
                myUIGroup.alpha -= Time.deltaTime*0.5f;
                if (myUIGroup.alpha <= 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    IEnumerator esperarFade()
    {
        Debug.Log("Estic esperant ");
        yield return new WaitForSeconds(6f);
    }

    private void unableText()
    {
        if (esTextPress)
        {
            myUIGroup.alpha = 0;
        }
    }
}
