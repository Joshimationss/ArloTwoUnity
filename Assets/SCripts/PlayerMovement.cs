using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode jumpKey;
    public KeyCode whipKey;
    public KeyCode grabKey = KeyCode.Joystick1Button1;

    [HideInInspector]
    public AudioSource audSource;

    [Header("Sounds")]
    public AudioClip jumpSnd;
    public AudioClip whipCrack;
    public AudioClip throwSnd;
    public AudioClip pickupSnd;

    [Header("Grab Related")]
    public bool isCarrying;
    public GrabRange grab;

    [Header("The Other")]
    public float moveSpeed;
    public Rigidbody rb;
    public float jumpForce = 10f;

    public Transform feet;
    public LayerMask groundLayers;

    public Animator anim;
    public State state;

    public enum State
    {
        normal,
        hold,
        toss,
        whip, // can grab rocks, and slap enemies
        swing, // to be used after whipping at points
        dead
    }

    bool isWhipping = true;

    [Header("Particles")]
    public ParticleSystem landFX;

    public bool isGrounded;
    float mx;
    float my;

    private void Awake()
    {
        grab = GetComponentInChildren<GrabRange>();
    }
    private void Start()
    {
        audSource = GetComponent<AudioSource>();
    }

    public void FixedUpdate()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");

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

        anim.SetBool("isGrounded", IsGrounded());
    }

    private void Update()
    {   
        switch (state)
        {
            case State.normal:
                isWhipping = false;
                anim.SetFloat("Speed", Mathf.Abs(mx));
                isCarrying = false;

                if (Input.GetKeyUp(whipKey))
                {
                    Whipping();

                    //state = State.whip;
                }

                if (Input.GetKeyDown(grabKey))
                {
                    if (grab.Pickup()) state = State.hold;
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

                    if (!audSource.isPlaying)
                    {
                        audSource.clip = throwSnd;
                        audSource.Play();
                    }

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
                isWhipping = true;
                break;
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
        Vector2 movement = new Vector2(rb.velocity.x, jumpForce);

        isGrounded = false;
        rb.velocity = movement;

        landFX.Play();

        if (!audSource.isPlaying)
        {
            if (!isGrounded == true)
            {
                audSource.clip = jumpSnd;
                audSource.Play();
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
        anim.SetTrigger("hasWhipped");

        if (!audSource.isPlaying)
        {
            audSource.clip = whipCrack;
            audSource.Play();
        }

    }
    public static bool AnimationOver(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f;
    }

}