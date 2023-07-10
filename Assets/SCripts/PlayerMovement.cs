using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode jumpKey;
    public KeyCode whipKey;

    [Header("Sounds")]
    private AudioSource audSource;
    public AudioClip jumpSnd;
    public AudioClip whipCrack;

    [Header("The Other")]
    public float moveSpeed;
    public Rigidbody rb;
    public float jumpForce = 10f;

    public Transform feet;
    public LayerMask groundLayers;

    public Animator anim;
    public State state;

    [Header("Particles")]
    public ParticleSystem landFX;

    public bool isGrounded;

    float mx;

    public bool isCarrying;


    private void Start()
    {
        audSource = GetComponent<AudioSource>();
    }

    public void FixedUpdate()
    {
        mx = Input.GetAxisRaw("Horizontal");

        if (isCarrying == false)
        {
            anim.SetFloat("Speed", Mathf.Abs(mx));
        } 
        else
        {
            anim.SetFloat("SpeedCarry", Mathf.Abs(mx));
        }

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

    public void LateUpdate()
    {
        Vector2 movement = new Vector2(mx * moveSpeed, rb.velocity.y);

        rb.velocity = movement;

        switch (state)
        {
            case State.normal:
                isCarrying = false;
            break;

            case State.grab:

            break;

            case State.hold:
                isCarrying = true;
            break;

            case State.toss:
                isCarrying = false;
            break;

            case State.dead:
                isCarrying = false;
            break;
        }

        if (Input.GetKey(jumpKey) && IsGrounded())
        {
            Jump();
        }

        if (Input.GetKeyUp(whipKey))
        {
            Whipping();
        }

    }

    public enum State
    {
        normal,
        grab,
        hold,
        toss,
        whip, // can grab rocks, and slap enemies
        swing, // to be used after whipping at points
        dead
    }

    void Jump()
    {
        Vector2 movement = new Vector2(rb.velocity.x, jumpForce);

        isGrounded = false;
        rb.velocity = movement;

        landFX.Play();

        if (!audSource.isPlaying)
        {
            if (isGrounded == true)
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
}