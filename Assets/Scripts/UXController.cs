using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UXController : MonoBehaviour
{
    [Header ("Objects")]
    [SerializeField]
    private GameObject grabButtonObj;
    [SerializeField]
    private GameObject attackButtonObj;
    [SerializeField]
    private GameObject nextButtonObj;
    [SerializeField]
    private GameObject lastButtonObj;
    [SerializeField]
    private GameObject dashButtonObj;


    [Header("Xbox")]
    [SerializeField]
    private Sprite attackButtonX;
    [SerializeField]
    private Sprite nextButtonX;
    [SerializeField]
    private Sprite lastButtonX;
    [SerializeField]
    private Sprite dashButtonX;
    [SerializeField]
    private Sprite grabButtonX;

    [Header("Playstation")]
    [SerializeField]
    private Sprite attackButtonP;
    [SerializeField]
    private Sprite nextButtonP;
    [SerializeField]
    private Sprite lastButtonP;
    [SerializeField]
    private Sprite dashButtonP;
    [SerializeField]
    private Sprite grabButtonP;

    [Header("PC")]
    [SerializeField]
    private Sprite attackButtonPC;
    [SerializeField]
    private Sprite nextButtonPC;
    [SerializeField]
    private Sprite lastButtonPC;
    [SerializeField]
    private Sprite dashButtonPC;
    [SerializeField]
    private Sprite grabButtonPC;

    public void activeGrabButton()
    {
        grabButtonObj.SetActive(!grabButtonObj.activeInHierarchy);
    }

    public void changeHUD(string controlls)
    {
        if(controlls == "Keyboard")
        {
            attackButtonObj.GetComponent<Image>().sprite = attackButtonPC;
            nextButtonObj.GetComponent<Image>().sprite = nextButtonPC;
            lastButtonObj.GetComponent<Image>().sprite = lastButtonPC;
            dashButtonObj.GetComponent<Image>().sprite = dashButtonPC;
            grabButtonObj.GetComponent<Image>().sprite = grabButtonPC;
        }
        else if(controlls == "Xbox")
        {
            attackButtonObj.GetComponent<Image>().sprite = attackButtonX;
            nextButtonObj.GetComponent<Image>().sprite = nextButtonX;
            lastButtonObj.GetComponent<Image>().sprite = lastButtonX;
            dashButtonObj.GetComponent<Image>().sprite = dashButtonX;
            grabButtonObj.GetComponent<Image>().sprite = grabButtonX;
        }
        else if(controlls == "Playstation")
        {
            attackButtonObj.GetComponent<Image>().sprite = attackButtonP;
            nextButtonObj.GetComponent<Image>().sprite = nextButtonP;
            lastButtonObj.GetComponent<Image>().sprite = lastButtonP;
            dashButtonObj.GetComponent<Image>().sprite = dashButtonP;
            grabButtonObj.GetComponent<Image>().sprite = grabButtonP;
        }
    }
}
