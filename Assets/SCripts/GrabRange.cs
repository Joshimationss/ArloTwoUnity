using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRange : MonoBehaviour
{
    public KeyCode grabKey;
    public Animator anim;
    public PlayerMovement pc;
    //public float tossHeight;
    //public float tossMagnitude;

    public Transform grabPoint;

    public bool buttonHeld;

    private GameObject rocka;

    private void Update()
    {
        if (pc.state == PlayerMovement.State.hold)
        {
            rocka.transform.position = grabPoint.transform.position;
        }

        if (!Input.GetKey(grabKey)) buttonHeld = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (pc.state != PlayerMovement.State.normal) return; //Get out if doing something else

        if (other.CompareTag("Rock") && Input.GetKey(grabKey))
        {
            rocka = other.gameObject;
            pc.state = PlayerMovement.State.hold;
            Debug.Log("touched");
        }
    }

    public void Toss(float direction)
    {
        rocka.GetComponent<RockScript>().Toss(direction);
        anim.SetTrigger("hasThrown");
    }

}
