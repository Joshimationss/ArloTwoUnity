using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipScript : MonoBehaviour
{
    private bool isActive = false;

    public Animator anim;
    public PlayerMovement pc;
    public GrabRange grab;
    public GameObject explosion;

    private GameObject rocka;

    private void Update()
    {
        if (rocka != null)
        {
            rocka.transform.position = transform.position;
        }
    }

    public void setActive(bool active)
    {
        Debug.Log("Set State " + active);

        isActive = active;
        rocka = null;
    }

    public bool Pickup()
    {
        return grab.Pickup(rocka);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject + ", " + isActive + ", " + rocka);
        if (other.CompareTag("Rock") && isActive && rocka == null)
        {
            Debug.Log("Set rock!");
            rocka = other.gameObject;
            rocka.GetComponent<RockScript>().move = RockScript.MoveState.held;
        }

        if (other.CompareTag("Enemy") && isActive == true)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Debug.Log("Blam");
            Destroy(other.gameObject); //Just damage the other enemy!
        }
    }
}
