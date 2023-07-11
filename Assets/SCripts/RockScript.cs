using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    public Rigidbody rb;
    public GrabRange gr;

    public TrailRenderer trail;

    public GameObject explosion;
    public MoveState move = MoveState.still;
    public enum MoveState
    {
        still,
        held,
        toss,
    }

    // Start is called before the first frame update
    private void Start()
    {
        gr = FindObjectOfType<GrabRange>();
    }

    private void Explode()
    {
        trail.emitting = false;
        Debug.Log("Blam");
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject); // :3
    }

    public void Toss(float direction) //Default magnitude of 10
    {
        Toss(direction, 10f, 8f);
        //rb.freezeRotation = false;
        trail.emitting = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Bonk
        if (collision.gameObject.CompareTag("Player") && move != MoveState.still && move != MoveState.held)
        {
            collision.gameObject.GetComponent<PlayerMovement>();
        }

        if (move == MoveState.toss)
        {
            Debug.Log("Kablam?");
            Explode();
        }

        //Stop falling
        if (Physics.Raycast(transform.position, Vector3.down, 0.6f))
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (move == MoveState.toss) move = MoveState.still; //Do not continue moving if thrown
        }
    }

    public void Toss(float direction, float height, float magnitude)
    {
        direction *= -Mathf.Deg2Rad; //Convert to radians, fix
        Vector3 force = new Vector3(Mathf.Cos(direction) * magnitude, height, Mathf.Sin(direction) * magnitude);
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
        rb.useGravity = true;
        move = MoveState.toss;
    }

    public static float GetAngle(float x, float y)
    {
        var angle = Mathf.Atan2(y, x);
        angle *= Mathf.Rad2Deg;
        return (360 + angle) % 360;
    }
}
