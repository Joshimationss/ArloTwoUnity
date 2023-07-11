using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRange : MonoBehaviour
{
    public Animator anim;
    public PlayerMovement pc;

    public Transform grabPoint;

    public bool buttonHeld;
    private GameObject rocka;

    private void Update()
    {
        if (pc.state == PlayerMovement.State.hold)
        {
            rocka.transform.position = grabPoint.transform.position;
        }

        if (!Input.GetKeyUp(pc.grabKey)) buttonHeld = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (pc.state != PlayerMovement.State.normal) return; //Get out if doing something else

        if (other.CompareTag("Rock") && Input.GetKeyUp(pc.grabKey))
        {
            rocka = other.gameObject;
            pc.state = PlayerMovement.State.hold;
            Debug.Log("touched");
            anim.SetBool("isCarrying", true);
        }
    }

    public void Toss(float direction)
    {
        Debug.Log("Throw");
        rocka.GetComponent<RockScript>().Toss(direction);
        anim.SetTrigger("hasThrown");
    }

}
