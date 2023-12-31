using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Health")]
    public float myHealth;
    public Image healthBar;

    [Header("Controls")]
    public KeyCode jumpKey;
    public KeyCode whipKey;
    public KeyCode grabKey = KeyCode.Joystick1Button1;

    [HideInInspector]
    public AudioSource audSource;

    [Header("WhipSounds")]
    public AudioClip[] whipCrack;
    [Header("OtherSounds")]
    public AudioClip jumpSnd;
    public AudioClip throwSnd;
    public AudioClip pickupSnd;
    public AudioClip ouchSound;
    public AudioClip healSound;

    [Header("Grab Related")]
    public bool isCarrying;
    public GrabRange grab;
    public WhipScript whip;

    [Header("Level Loading")]
    public LevelLoader lL;

    [Header("The Other")]
    public float moveSpeed;
    public Rigidbody rb;
    public float jumpForce = 10f;
    public Transform feet;
    public LayerMask groundLayers;
    public Animator anim;
    public State state;
    [HideInInspector]
    public bool enteringDoor;

    public enum State
    {
        normal,
        hold,
        toss,
        whip, // can grab rocks, and slap enemies
        swing, // to be used after whipping at points
        dead
    }

    [Header("Particles")]
    public ParticleSystem landFX;

    public bool isGrounded;
    float mx;
    float my;

    private bool isDead;

    private void Awake()
    {
        lL = GetComponent<LevelLoader>();
        grab = GetComponentInChildren<GrabRange>();
    }
    private void Start()
    {
        enteringDoor = false;
        isDead = false;
        audSource = GetComponent<AudioSource>();
    }

    public void FixedUpdate()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");

        if (enteringDoor == false)
        {
            if (isDead == false)
            {
                if (mx > 0)
                {
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    transform.rotation = rotation;
                }
                else if (mx < 0)
                {
                    Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
                    transform.rotation = rotation;
                }
            }
            anim.SetBool("isGrounded", IsGrounded());
        }
    }

    private void Update()
    {
        if (my > 0.5f)
        {
            enteringDoor = true;
        }
        else
        {
            enteringDoor = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(1);
        }

        switch (state)
        {
            case State.normal:
                anim.SetFloat("Speed", Mathf.Abs(mx));
                isCarrying = false;

                if (Input.GetKeyDown(whipKey))
                {
                   Whipping();
                }

                if (Input.GetKeyDown(grabKey))
                {
                    if (isDead == false)
                    {
                        if (grab.Pickup()) state = State.hold;
                    }

                }
                break;

            case State.hold:
                anim.SetFloat("Speed", Mathf.Abs(mx));
                isCarrying = true;

                if (Input.GetKeyDown(grabKey)) //Toss it!
                {
                    anim.SetBool("isCarrying", false);
                    state = State.toss;
                    grab.Toss(transform.rotation.eulerAngles.y); // + 270

                    audSource.clip = throwSnd;
                    audSource.Play();
                }

                if (my < -0.5) //Drop it!
                {
                    anim.SetBool("isCarrying", false);
                    state = State.normal;
                    grab.Drop();
                }
                break;

            case State.toss:
                if (AnimationOver(anim)) state = State.normal;
                isCarrying = false;
                break;

            case State.dead:
                isCarrying = false;
                break;

            case State.whip:
                if (AnimationOver(anim) && AnimatorOnState(anim, "Whip"))
                {
                    state = whip.Pickup() ? State.hold : State.normal;
                    whip.setActive(false);
                }
                break;
        }


        if (myHealth < 1)
        {
            StartCoroutine(DeathTimer());
        }

    }

    public void LateUpdate()
    {
        if (state == State.hold)
        {
            Vector2 movement = new Vector2(mx * moveSpeed / 1.7f, rb.velocity.y);
            rb.velocity = movement;
        }
        else
        {
            Vector2 movement = new Vector2(mx * moveSpeed, rb.velocity.y);
            rb.velocity = movement;
        }


        if (Input.GetKey(jumpKey) && IsGrounded())
        {
            Jump();
        }
    }

    void Jump()
    {
        if (isDead == false || enteringDoor == false)
        {
            Vector2 movement = new Vector2(rb.velocity.x, jumpForce);

            isGrounded = false;
            rb.velocity = movement;

            landFX.Play();

            if (!isGrounded == true || enteringDoor == false)
            {
                if(!audSource.isPlaying)
                {
                    audSource.clip = jumpSnd;
                    audSource.Play();
                }
                
            } 
        }
    }

    public bool IsGrounded()
    {
        Collider[] groundCheck = Physics.OverlapSphere(feet.position, 0.1f, groundLayers.value);
        isGrounded = true;
        return groundCheck.Length > 0;
    }

    void Whipping()
    {
        int rand = UnityEngine.Random.Range(0, whipCrack.Length);
        audSource.clip = whipCrack[rand];
        audSource.Play();

        if (!isDead || !enteringDoor)
        {
            if (isGrounded == true)
            {
                whip.setActive(true);
                anim.SetTrigger("hasWhipped");
                state = State.whip;
            }  
        }
    }

    void StopMovement()
    {
        moveSpeed = 0;
        jumpForce = 0;
        isDead = false;
        isCarrying = false;
    }

    public static bool AnimationOver(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f;
    }

    public static bool AnimatorOnState(Animator animator, String state)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            TakeDamage(1);
        }

        if (collision.collider.tag == "Food")
        {
            Destroy(collision.collider.gameObject);
            Heal(1);
        }
    }

    public void TakeDamage(float damage)
    {
        audSource.clip = ouchSound;
        audSource.Play();
        myHealth -= damage;
        healthBar.fillAmount = myHealth / 6f;
        anim.SetTrigger("beenHurt");
    }

    public void Heal(float healAmount)
    {
        audSource.clip = healSound;
        audSource.Play();
        myHealth += healAmount;
        myHealth = Mathf.Clamp(myHealth, 0, 6);
        healthBar.fillAmount = myHealth / 6f;
    }

    IEnumerator DeathTimer()
    {
        isDead = true;
        anim.SetBool("hasDied", true);
        moveSpeed = 0;
        jumpForce = 0;
        rb.isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(4f);
        Debug.Log("Bleep");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EnterDoor()
    {
        state = State.toss;
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = rotation;
        Debug.Log("Enter Door");
        anim.SetTrigger("EnteringDoor");
        StopMovement();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FakeWall"))
        {
            other.GetComponent<Renderer>().enabled = false;
        } 
    }
}