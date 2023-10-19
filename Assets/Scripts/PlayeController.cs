using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayeController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private PlayerControlls playerControlls;
    private Rigidbody2D rigidbody;
    private Vector2 moveInput;

    private string currentControllScheme;


    private void Awake()
    {
        playerControlls = new PlayerControlls();
        rigidbody = GetComponent<Rigidbody2D>();

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
        rigidbody.velocity = moveInput * speed;
    }

    private void SwitchControls(PlayerInput input)
    {
        currentControllScheme = input.currentControlScheme;
        Debug.Log("Device is now: " + currentControllScheme);
    }
}
