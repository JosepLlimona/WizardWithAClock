using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
    public void OnButtonClick()
    {
        // Load the main scene (you can replace "MainScene" with the actual scene name)
        SceneManager.LoadScene("TestScene");
    }
    public void OnEndButtonClick()
    {
        // Load the main scene (you can replace "MainScene" with the actual scene name)
        //tanca el joc
        Application.Quit();
    }
}
