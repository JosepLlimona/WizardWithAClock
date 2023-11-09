using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private Animator clockAnim;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private TextMeshProUGUI text;

    private int actualClock = 1;

    private bool performed = false;
    private bool dashPerformed = false;
    private bool canMove = true;

    private string currentControllScheme;


    private void Awake()
    {
        playerControlls = new PlayerControlls();
        rbody = GetComponent<Rigidbody2D>();

        playerInput.onControlsChanged += SwitchControls;
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
            playerAnim.SetFloat("dirX", moveInput.x);
            playerAnim.SetFloat("dirY", moveInput.y);
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
        playerControlls.Standard.Attack.performed += context =>
        {
            Debug.Log("Attacking");
            StartCoroutine(attack());
        };
        playerControlls.Standard.NextClock.performed += context =>
        {
            actualClock++;
            if(actualClock >= 4)
            {
                actualClock = 1;
            }
            changeText();
            Debug.Log("Actual Clock: " + actualClock);
        };
    }

    private void changeText()
    {
        if (actualClock == 1)
        {
            text.text = "Actual clock: Light";
        }
        else if(actualClock == 2)
        {
            text.text = "Actual clock: Medium";
        }
        else if(actualClock == 3)
        {
            text.text = "Actual clock: Heavy";
        }
    }

    private IEnumerator dash(Vector2 direction)
    {
        canMove = false;
        playerAnim.SetBool("dashing", true);
        if (direction == Vector2.zero) 
        { 
           direction = Vector2.left;
        };
        rbody.AddForce(direction * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        playerAnim.SetBool("dashing", false);
        canMove = true;
    }

    private IEnumerator attack()
    {
        if (actualClock == 1)
        {
            clockAnim.SetBool("LightAttack", true);
            yield return new WaitForSeconds(1f);
            clockAnim.SetBool("LightAttack", false);
        }
        else if (actualClock == 2)
        {
            clockAnim.SetBool("MediumAttack", true);
            yield return new WaitForSeconds(1.5f);
            clockAnim.SetBool("MediumAttack", false);
        }
        else if (actualClock == 3)
        {
            clockAnim.SetBool("HeavyAttack", true);
            yield return new WaitForSeconds(2f);
            clockAnim.SetBool("HeavyAttack", false);
        }
    }

    private void SwitchControls(PlayerInput input)
    {
        currentControllScheme = input.currentControlScheme;
        Debug.Log("Device is now: " + currentControllScheme);
    }
}
