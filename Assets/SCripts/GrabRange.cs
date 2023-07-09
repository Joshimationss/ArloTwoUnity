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

    private GameObject bomba;

    private void Update()
    {
        if (pc.state == PlayerMovement.State.hold)
        {
            bomba.transform.position = grabPoint.transform.position;
        }

        if (!Input.GetKey(grabKey)) buttonHeld = false;
    }

    /*

    public void OnTriggerStay(Collider other)
    {
        if (pc.state != PlayerMovement.State.normal) return; //Get out if doing something else

        if (other.CompareTag("Rock") && Input.GetKey(grabKey))
        {
            bomba = other.gameObject;
            bomba.GetComponent<BombExplode>().move = BombExplode.MoveState.held;
            pc.state = PlayerMovement.State.hold; //TODO: Change to grab when animation is implemented
            pc.grabWait = true;
            //anim.SetBool("isCarrying", true);
        }
    }

    public void Toss(float direction)
    {
        bomba.GetComponent<BombExplode>().Toss(direction);
        anim.SetTrigger("isToss");
    }

    */
}
