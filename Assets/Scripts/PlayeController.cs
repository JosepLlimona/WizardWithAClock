using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayeController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private PlayerControlls playerControlls;
    private Rigidbody2D rbody;
    private Vector2 moveInput;

    private bool performed = false;

    private string currentControllScheme;


    private void Awake()
    {
        playerControlls = new PlayerControlls();
        rbody = GetComponent<Rigidbody2D>();

        InputManager.instance.playerInput.onControlsChanged += SwitchControls;
    }

    private void OnEnable()
    {
        playerControlls.Standard.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Standard.Disable();
    }

    private void FixedUpdate()
    {
        moveInput = playerControlls.Standard.Movement.ReadValue<Vector2>();
        rbody.velocity = moveInput * speed;

        playerControlls.Standard.Dash.performed += context =>
        {
            performed = true;
            if(context.interaction is TapInteraction)
            {
                Debug.Log("Dashing");
            }
            else
            {
                Debug.Log("Running");
            }
        };
        playerControlls.Standard.Dash.canceled += context =>
        {
            if (performed && context.interaction is HoldInteraction)
            {
                Debug.Log("End Running");
            }
        };
    }
    private void SwitchControls(PlayerInput input)
    {
        currentControllScheme = input.currentControlScheme;
        Debug.Log("Device is now: " + currentControllScheme);
    }
}
