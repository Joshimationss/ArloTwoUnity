using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRange : MonoBehaviour
{
    public Animator anim;
    public PlayerMovement pc;

    public Transform grabPoint;

    private GameObject rocka;

    private void Update()
    {
        if (pc.state == PlayerMovement.State.hold)
        {
            rocka.transform.position = grabPoint.transform.position;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (pc.state == PlayerMovement.State.hold) return; //Get out
        rocka = null;
        if (pc.state != PlayerMovement.State.normal) return; //Get out
        if (other.CompareTag("Rock")) rocka = other.gameObject;
    }

    public bool Pickup()
    {
        if (rocka != null)
        {
            if (!pc.audSource.isPlaying)
            {
                pc.audSource.clip = pc.pickupSnd;
                pc.audSource.Play();
            }

            Debug.Log("touched");
            anim.SetBool("isCarrying", true);
            return true;
        }
        return false;
    }

    public void Toss(float direction)
    {
        Debug.Log("Throw");
        rocka.GetComponent<RockScript>().Toss(direction);
        anim.SetTrigger("hasThrown");
    }

    public void Drop()
    {
        Debug.Log("Drop");
        rocka.GetComponent<RockScript>().Drop();
    }
}
