using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField]
    private GameObject continueButton;
    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject closeButton;
    [SerializeField]
    private GameObject itemButton;


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
    [SerializeField]
    private Sprite itemButtonX;


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
    [SerializeField]
    private Sprite itemButtonP;

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
    [SerializeField]
    private Sprite itemButtonPC;

    private bool paused = false;

    private PlayerControlls playerControlls;

    private void Awake()
    {
        playerControlls = new PlayerControlls();

        playerControlls.Standard.Pause.performed += context =>
        {
            continueGame();
        };
    }

    private void OnEnable()
    {
        playerControlls.Standard.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Standard.Disable();
    }


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
            itemButton.GetComponent<Image>().sprite = itemButtonPC;
        }
        else if(controlls == "Xbox")
        {
            attackButtonObj.GetComponent<Image>().sprite = attackButtonX;
            nextButtonObj.GetComponent<Image>().sprite = nextButtonX;
            lastButtonObj.GetComponent<Image>().sprite = lastButtonX;
            dashButtonObj.GetComponent<Image>().sprite = dashButtonX;
            grabButtonObj.GetComponent<Image>().sprite = grabButtonX;
            itemButton.GetComponent<Image>().sprite = itemButtonX;
        }
        else if(controlls == "Playstation")
        {
            attackButtonObj.GetComponent<Image>().sprite = attackButtonP;
            nextButtonObj.GetComponent<Image>().sprite = nextButtonP;
            lastButtonObj.GetComponent<Image>().sprite = lastButtonP;
            dashButtonObj.GetComponent<Image>().sprite = dashButtonP;
            grabButtonObj.GetComponent<Image>().sprite = grabButtonP;
            itemButton.GetComponent<Image>().sprite = itemButtonP;
        }
    }

    public void continueGame()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        continueButton.SetActive(paused);
        exitButton.SetActive(paused);
        closeButton.SetActive(paused);
    }

    public void exitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void closeGame()
    {
        Application.Quit();
    }
}
