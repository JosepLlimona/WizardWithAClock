using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [HideInInspector]
    public PlayerControlls playerControlls;
    public PlayerInput playerInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerControlls = new PlayerControlls();

        playerInput = GetComponent<PlayerInput>(); 
    }

    private void OnEnable()
    {
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }
}
