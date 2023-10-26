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
    [SerializeField]
    private float dashForce;

    private bool performed = false;
    private bool dashPerformed = false;
    private bool canMove = true;

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
        if (canMove)
        {
            rbody.velocity = moveInput * speed;
        }
        playerControlls.Standard.Dash.performed += context =>
        {
            if (context.interaction is TapInteraction)
            {
                rbody.velocity = Vector2.zero;
                StartCoroutine(dash(moveInput));
            }
            else
            {
                performed = true;
                speed *= 2;
                Debug.Log("Running");
            }
        };
        playerControlls.Standard.Dash.canceled += context =>
        {
            if (performed && context.interaction is HoldInteraction)
            {
                speed /= 2;
                performed = false;
                Debug.Log("End Running. Speed: " + speed);
            }
        };
    }

    private IEnumerator dash(Vector2 direction)
    {
        canMove = false;
        if (direction == Vector2.zero) 
        { 
           direction = Vector2.left;
        };
        rbody.AddForce(direction * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }
    private void SwitchControls(PlayerInput input)
    {
        currentControllScheme = input.currentControlScheme;
        Debug.Log("Device is now: " + currentControllScheme);
    }
}
