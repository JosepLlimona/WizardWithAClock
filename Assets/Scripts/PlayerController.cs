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
    [Header("Movement")]
    [SerializeField]
    private float speed;
    private PlayerControlls playerControlls;
    private Rigidbody2D rbody;
    private Vector2 moveInput;
    [SerializeField] private bool esMirror;
    [SerializeField]
    private float dashForce;
    private bool isDashing = false;

    [Header("Positions")]
    [SerializeField]
    private GameObject leftPos;
    [SerializeField]
    private GameObject downPos;
    [SerializeField]
    private GameObject rightPos;
    [SerializeField]
    private GameObject upPos;
    [SerializeField]
    private GameObject upDPos;
    [SerializeField]
    private GameObject downDPos;

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
    private GameObject sword;
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

    [Header("Items")]
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Sprite sBalaStop;
    [SerializeField]
    private Sprite sLastHit;
    [SerializeField]
    private Sprite sBrokenClock;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject brokenClock;
    private bool hasBalaStop = false;
    private bool hasLastHit = false;
    private bool hasBrokenClock = false;
    private bool canShot = false;
    [SerializeField]
    private int cooldown = 5;
    private int lastHit;

    private int actualClock = 1;

    private bool dashPerformed = false;
    private bool mattackPerformed = false;
    private bool canMove = true;
    [SerializeField] private bool canHeavyAttack = true;
    private int combo = 0;

    private bool grabItem = false;
    private GameObject itemGrabed = null;
    private int swordDamage = 15;
    private int hammerDamage = 30;

    private string currentControllScheme;


    private void Awake()
    {
        hammerStart = leftPos.transform;
        //if (esMirror) { this.gameObject.SetActive(false); }
        playerControlls = new PlayerControlls();
        rbody = GetComponent<Rigidbody2D>();

        // Assegura't que playerInput estigui inicialitzat
        if (playerInput != null)
        {
            Debug.Log("Afegint");
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
            }
        };
        playerControlls.Standard.Dash.canceled += context =>
        {
            if (dashPerformed && context.interaction is HoldInteraction)
            {
                speed /= 2;
                playerAnim.speed = 1;
                dashPerformed = false;
            }
        };
        playerControlls.Standard.Attack.performed += context =>
        {
            if (context.interaction is TapInteraction)
            {
                if (canHeavyAttack || actualClock != 3)
                {
                    StartCoroutine(attack());
                }
            }
            else if (context.interaction is HoldInteraction && actualClock == 1)
            {
                mattackPerformed = true;
                fistAnim.SetBool("isAttacking", true);
                clockAnim.SetBool("LightAttack", true);
                fists.GetComponent<SpriteRenderer>().enabled = true;
                //ora.Play();
            }
        };
        playerControlls.Standard.Attack.canceled += context =>
        {
            if (context.interaction is HoldInteraction && mattackPerformed && actualClock == 1)
            {
                mattackPerformed = false;
                fists.GetComponent<SpriteRenderer>().enabled = false;
                /*ora.Stop();
                singleOra.Play();*/
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

        playerControlls.Standard.Grab.performed += context =>
        {
            if (grabItem)
            {
                Debug.Log("Agafat");
                itemGrabed.GetComponent<Items>().activeItem(this.gameObject);
            }
        };

        playerControlls.Standard.Item.performed += context =>
        {
            if (hasBalaStop && canShot)
            {
                GameObject bulletInstance = Instantiate(bullet, hammerStart.position, hammerStart.rotation);
                bulletInstance.transform.localScale = transform.localScale;
                Vector2 dir = hammerStart.position - transform.position;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = dir.normalized * 3;
                StartCoroutine(shootCooldown());
            }
            if (hasLastHit && canShot)
            {
                heal(lastHit);
                StartCoroutine(shootCooldown());
            }
            if(hasBrokenClock && canShot)
            {
                brokenClock.SetActive(true);
                StartCoroutine(shootCooldown());
            }
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
            if (moveInput == Vector2.zero)
            {
                playerAnim.SetBool("isWalking", false);
            }
            else
            {
                playerAnim.SetBool("isWalking", true);
            }
            if (moveInput.x < 0)
            {
                fists.transform.position = leftPos.transform.position;
                fists.transform.rotation = leftPos.transform.rotation;
                fists.transform.localScale = leftPos.transform.localScale;
                sword.transform.position = leftPos.transform.position;
                sword.transform.rotation = leftPos.transform.rotation;
                sword.transform.localScale = leftPos.transform.localScale;
                hammerStart = leftPos.transform;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (moveInput.x > 0)
            {
                fists.transform.position = leftPos.transform.position;
                fists.transform.rotation = leftPos.transform.rotation;
                fists.transform.localScale = leftPos.transform.localScale;
                sword.transform.position = leftPos.transform.position;
                sword.transform.rotation = leftPos.transform.rotation;
                sword.transform.localScale = leftPos.transform.localScale;
                hammerStart = leftPos.transform;
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (moveInput.y > 0)
            {
                fists.transform.position = upPos.transform.position;
                fists.transform.rotation = upPos.transform.rotation;
                fists.transform.localScale = upPos.transform.localScale;
                sword.transform.position = upPos.transform.position;
                sword.transform.rotation = upPos.transform.rotation;
                sword.transform.localScale = upPos.transform.localScale;
                hammerStart = upPos.transform;
            }
            else if (moveInput.y < 0)
            {
                fists.transform.position = downPos.transform.position;
                fists.transform.rotation = downPos.transform.rotation;
                fists.transform.localScale = downPos.transform.localScale;
                sword.transform.position = downPos.transform.position;
                sword.transform.rotation = downPos.transform.rotation;
                sword.transform.localScale = downPos.transform.localScale;
                hammerStart = downPos.transform;
            }

            if ((moveInput.x > 0 || moveInput.x < 0) && moveInput.y > 0)
            {
                fists.transform.position = upDPos.transform.position;
                fists.transform.rotation = upDPos.transform.rotation;
                fists.transform.localScale = upDPos.transform.localScale;
                sword.transform.position = upDPos.transform.position;
                sword.transform.rotation = upDPos.transform.rotation;
                sword.transform.localScale = upDPos.transform.localScale;
                hammerStart = upDPos.transform;
            }
            else if ((moveInput.x > 0 || moveInput.x < 0) && moveInput.y < 0)
            {
                fists.transform.position = downDPos.transform.position;
                fists.transform.rotation = downDPos.transform.rotation;
                fists.transform.localScale = downDPos.transform.localScale;
                sword.transform.position = downDPos.transform.position;
                sword.transform.rotation = downDPos.transform.rotation;
                sword.transform.localScale = downDPos.transform.localScale;
                hammerStart = downDPos.transform;
            }

            playerAnim.SetFloat("dirX", moveInput.x);
            playerAnim.SetFloat("dirY", moveInput.y);
            clockAnim.SetFloat("dirX", moveInput.x);
            clockAnim.SetFloat("dirY", moveInput.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Item")
        {
            GameObject.Find("HUD").GetComponent<UXController>().activeGrabButton();
            Debug.Log("Entro");
            grabItem = true;
            itemGrabed = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Item")
        {
            GameObject.Find("HUD").GetComponent<UXController>().activeGrabButton();
            Debug.Log("Surto");
            grabItem = false;
            itemGrabed = null;
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
        canHeavyAttack = true;
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

            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            GameObject hammerInstance = Instantiate(hammer, pos, Quaternion.identity);
            hammerInstance.GetComponent<HammerController>().setDamage(hammerDamage);
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
                swordTAttack.GetComponent<SwordThirdController>().setPlayer(this.gameObject);
                swordTAttack.GetComponent<SwordThirdController>().setDamage(swordDamage);
                break;
            case "ThirdAttack":
                clockAnim.SetBool("MediumAttack", false);
                combo = 0;
                break;
        }
    }

    public void lostLife(int damage)
    {
        lastHit = damage;
        playerAnim.SetTrigger("Hurt");
        life -= damage;
        if (life <= 0)
        {
            life = 0;
            Debug.Log("Game Over");
            Application.Quit();
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


    //Item functions
    public void changeSpeed(int newSpeed)
    {
        this.speed = newSpeed;
    }

    public void changeWeaponDamage(int newDamage, string weapon)
    {
        if (weapon == "fists")
        {
            transform.Find("Fists").GetComponent<FistsController>().setDamage(newDamage);
        }
        else if (weapon == "sword")
        {
            transform.Find("Sword").GetComponent<SwordController>().setDamage(newDamage);
            swordDamage = newDamage;
        }
        else if (weapon == "hammer")
        {
            hammerDamage += newDamage;
        }
    }

    public void moreLife(int life)
    {
        maxLife += life;
        this.life += life;
        lifeSlider.maxValue = maxLife;
        lifeSlider.value = this.life;
        lifeText.text = this.life.ToString();
    }

    public void activeItem(string itemName)
    {
        Debug.Log("Activant: " + itemName);
        switch (itemName)
        {
            case "BalaStop":
                Debug.Log("Entro case");
                hasBalaStop = true;
                hasLastHit = false;
                hasBrokenClock = false;
                canShot = true;
                itemImage.sprite = sBalaStop;
                itemImage.enabled = true;
                break;
            case "LastHit":
                hasLastHit = true;
                hasBalaStop = false;
                hasBrokenClock = false;
                canShot = true;
                itemImage.sprite = sLastHit;
                itemImage.enabled = true;
                break;
            case "BrokenClock":
                hasBrokenClock = true;
                hasBalaStop = false;
                hasLastHit = false;
                canShot = true;
                itemImage.sprite = sBrokenClock;
                itemImage.enabled = true;
                break;

        }

    }

    private IEnumerator shootCooldown()
    {
        Color tempColor = itemImage.color;
        tempColor.a = 0.5f;
        itemImage.color = tempColor;
        canShot = false;
        yield return new WaitForSeconds(cooldown);
        canShot = true;
        tempColor.a = 1f;
        itemImage.color = tempColor;
    }

    private void SwitchControls(PlayerInput input)
    {
        currentControllScheme = input.currentControlScheme;
        Debug.Log("Device is now: " + currentControllScheme);
        GameObject.Find("HUD").GetComponent<UXController>().changeHUD(currentControllScheme);
    }

    public void disableMirrorPlayer()
    {
        if (esMirror)
        {
            gameObject.SetActive(false);

        }
    }

    public void setPosition(Vector3 pos){
        gameObject.transform.position = pos;
    }

}
