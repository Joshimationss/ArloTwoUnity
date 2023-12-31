using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRange : MonoBehaviour
{
    public Animator anim;
    public PlayerMovement pc;
    public WhipScript ws;
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

    public bool Pickup(GameObject rocka)
    {
        this.rocka = rocka;
        return Pickup();
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
            anim.SetBool("isCarrying", true);
            return true;
        }
        return false;
    }

    public void Toss(float direction)
    {
        rocka.GetComponent<RockScript>().Toss(direction);
        anim.SetTrigger("hasThrown");
    }

    public void Drop()
    {
        rocka.GetComponent<RockScript>().Drop();
    }
}
