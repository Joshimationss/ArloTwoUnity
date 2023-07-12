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
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject); // :3
    }

    public void Toss(float direction) //Default magnitude of 10
    {
        Toss(direction, 10f, 8f);
        //rb.freezeRotation = false;
        trail.emitting = true;
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

    public void Drop()
    {
        move = MoveState.still;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (move == MoveState.toss)
        {
            Explode();
        }

        if (move == MoveState.toss && collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }

    }


    public static float GetAngle(float x, float y)
    {
        var angle = Mathf.Atan2(y, x);
        angle *= Mathf.Rad2Deg;
        return (360 + angle) % 360;
    }
}
