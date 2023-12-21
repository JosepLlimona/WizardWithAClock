using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    [Header ("Movement")]
    [SerializeField]
    private float speed;
    private PlayerControlls playerControlls;
    private Rigidbody2D rbody;
    private Vector2 moveInput;
    [SerializeField] private bool esMirror;
    [SerializeField]
    private float dashForce;
    private bool isDashing = false;

    [Header("Animators")]
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private Animator clockAnim;
    [SerializeField]
    private Animator fistAnim;
    [SerializeField]
    private Animator swordAnim;
    [Header("Input")]
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private TextMeshProUGUI text;
    
    [Header("GameObjects")]
    [SerializeField]
    private GameObject hammer;
    [SerializeField]
    private GameObject swordThird;
    [SerializeField]
    private GameObject fists;
    [SerializeField]
    private Transform hammerStart;

    [Header("Audio")]
    [SerializeField]
    private AudioSource ora;
    [SerializeField]
    private AudioSource singleOra;
    [SerializeField]
    private AudioSource[] punches;
    [SerializeField]
    private AudioSource[] misses;

    [Header("Health")]
    [SerializeField]
    private Slider lifeSlider;
    [SerializeField]
    private TextMeshProUGUI lifeText;
    private int life = 100;
    private int maxLife = 100;

    private int actualClock = 1;

    private bool dashPerformed = false;
    private bool mattackPerformed = false;
    private bool canMove = true;
    private bool canHeavyAttack = true;
    private int combo = 0;

    private string currentControllScheme;


    private void Awake()
    {
        //if (esMirror) { this.gameObject.SetActive(false); }
        playerControlls = new PlayerControlls();
        rbody = GetComponent<Rigidbody2D>();

        // Assegura't que playerInput estigui inicialitzat
        if (playerInput != null)
        {
            playerInput.onControlsChanged += SwitchControls;
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
                dashPerformed = true;
                speed *= 2;
                playerAnim.speed = 2;
                Debug.Log("Running");
            }
        };
        playerControlls.Standard.Dash.canceled += context =>
        {
            if (dashPerformed && context.interaction is HoldInteraction)
            {
                speed /= 2;
                playerAnim.speed = 1;
                dashPerformed = false;
                Debug.Log("End Running. Speed: " + speed);
            }
        };
        playerControlls.Standard.Attack.performed += context =>
        {
            if (context.interaction is TapInteraction)
            {
                Debug.Log("Attacking");
                if (canHeavyAttack)
                {
                    StartCoroutine(attack());
                }
            }
            else if (context.interaction is HoldInteraction && actualClock == 1)
            {
                Debug.Log("Starting attacking");
                mattackPerformed = true;
                fistAnim.SetBool("isAttacking", true);
                clockAnim.SetBool("LightAttack", true);
                fists.GetComponent<SpriteRenderer>().enabled = true;
                ora.Play();
            }
        };
        playerControlls.Standard.Attack.canceled += context =>
        {
            if (context.interaction is HoldInteraction && mattackPerformed && actualClock == 1)
            {
                Debug.Log("Attack stoped");
                mattackPerformed = false;
                fists.GetComponent<SpriteRenderer>().enabled = false;
                ora.Stop();
                singleOra.Play();
                fistAnim.SetBool("isAttacking", false);
                clockAnim.SetBool("LightAttack", false);
            }
        };
        playerControlls.Standard.NextClock.performed += context =>
        {
            actualClock++;
            if (actualClock >= 4)
            {
                actualClock = 1;
            }
            StopAllCoroutines();
            resetBools();
            changeText();
            Debug.Log("Actual Clock: " + actualClock);
        };
        playerControlls.Standard.LastClock.performed += context =>
        {
            actualClock--;
            if (actualClock <= 0)
            {
                actualClock = 3;
            }
            StopAllCoroutines();
            resetBools();
            changeText();
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

    private void FixedUpdate()
    {
     
        if (esMirror)
        {
            moveInput = playerControlls.Standard.Movement.ReadValue<Vector2>();
            moveInput.y *= -1;
        }
        else
        {
            moveInput = playerControlls.Standard.Movement.ReadValue<Vector2>();
        }
        if (canMove)
        {
            rbody.velocity = moveInput * speed;
            if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            playerAnim.SetFloat("dirX", moveInput.x);
            playerAnim.SetFloat("dirY", moveInput.y);
            clockAnim.SetFloat("dirX", moveInput.x);
            clockAnim.SetFloat("dirY", moveInput.y);
        }
    }

    private void changeText()
    {
        if (actualClock == 1)
        {
            text.text = "Actual clock: Light";
        }
        else if (actualClock == 2)
        {
            text.text = "Actual clock: Medium";
        }
        else if (actualClock == 3)
        {
            text.text = "Actual clock: Heavy";
        }
    }

    private void resetBools()
    {
        clockAnim.SetBool("LightAttack", false);
        fistAnim.SetBool("isAttacking", false);
        clockAnim.SetBool("MediumAttack", false);
        clockAnim.SetBool("HeavyAttack", false);
        mattackPerformed = false;
        ora.Stop();
        singleOra.Stop();
        fists.GetComponent<SpriteRenderer>().enabled = false;
    }
    private IEnumerator dash(Vector2 direction)
    {
        if (!isDashing)
        {
            isDashing = true;
            canMove = false;
            Physics2D.IgnoreLayerCollision(8, 9, true);
            playerAnim.SetBool("dashing", true);
            if (direction == Vector2.zero)
            {
                direction = Vector2.left;
            };
            rbody.AddForce(direction * dashForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.3f);
            playerAnim.SetBool("dashing", false);
            canMove = true;
            Physics2D.IgnoreLayerCollision(8, 9, false);
            isDashing = false;
        }
    }

    private IEnumerator attack()
    {
        if (actualClock == 1)
        {
            fists.GetComponent<SpriteRenderer>().enabled = true;
            clockAnim.SetBool("LightAttack", true);
            fistAnim.SetBool("isAttacking", true);
            yield return new WaitForSeconds(1f);
            fistAnim.SetBool("isAttacking", false);
            clockAnim.SetBool("LightAttack", false);
            fists.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (actualClock == 2)
        {
            clockAnim.SetBool("MediumAttack", true);
            if (combo < 3)
            {
                combo++;
            }
            swordAttack();
        }
        else if (actualClock == 3)
        {
            canHeavyAttack = false;
            Vector3 pos = hammerStart.transform.position;
            GameObject hammerInstance = Instantiate(hammer, pos, Quaternion.identity);
            clockAnim.SetBool("HeavyAttack", true);
            hammerInstance.GetComponent<Animator>().SetTrigger("Attacking");
            yield return new WaitForSeconds(2f);
            clockAnim.SetBool("HeavyAttack", false);
            canHeavyAttack = true;
            yield return new WaitForSeconds(2f);
            Destroy(hammerInstance);
        }
    }

    public void playPunchAudio(bool hit)
    {
        if (hit)
        {
            int rand = Random.Range(1, 6);
            if (punches[rand].isPlaying)
            {
                rand = Random.Range(1, 6);
            }
            punches[rand].Play();
        }
        else
        {
            int rand = Random.Range(1, 6);
            if (misses[rand].isPlaying)
            {
                rand = Random.Range(1, 6);
            }
            misses[rand].Play();
        }
    }

    private void swordAttack()
    {
        if (combo >= 2)
        {
            return;
        }
        swordAnim.SetTrigger("FirstAttack");
    }

    public void continueCombo(string actual)
    {
        Debug.Log("Entering with: " + actual);
        switch (actual)
        {
            case "FirstAttack": 
                if (combo <= 1)
                {
                    clockAnim.SetBool("MediumAttack", false);
                    combo = 0;
                    return;
                }
                swordAnim.SetTrigger("SecondAttack");
                break;
            case "SecondAttack":
                if (combo <= 2)
                {
                    clockAnim.SetBool("MediumAttack", false);
                    combo = 0;
                    return;
                }
                Vector3 pos = hammerStart.transform.position;
                var swordTAttack = Instantiate(swordThird, pos, Quaternion.identity);
                swordTAttack.GetComponent<SwordThirdController>().player = this.gameObject;
                break;
            case "ThirdAttack":
                Debug.Log("Enters");
                if (!clockAnim.isActiveAndEnabled)
                {
                    Debug.Log("Animator disabled");
                }
                clockAnim.SetBool("MediumAttack", false);
                combo = 0;
                break;
        }
    }

    public void lostLife(int damage)
    {
        life -= damage;
        if(life <= 0)
        {
            life = 0;
            Debug.Log("Game Over");
        }
        lifeSlider.value = life;
        lifeText.text = life.ToString();
    }

    public void heal(int healing)
    {
        life += healing;
        if (life >= maxLife)
        {
            life = maxLife;
        }
        lifeSlider.value = life;
        lifeText.text = life.ToString();
    }

    public void endSTA()
    {
        continueCombo("ThirdAttack");
    }

    private void SwitchControls(PlayerInput input)
    {
        currentControllScheme = input.currentControlScheme;
        Debug.Log("Device is now: " + currentControllScheme);
    }

    public void disableMirrorPlayer()
    {
        if (esMirror)
        {
           gameObject.SetActive(false);
           
        }
    }
    
}
