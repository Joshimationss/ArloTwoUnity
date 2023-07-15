using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipScript : MonoBehaviour
{
    public GameObject whipBox;
    public GrabRange gr;
    public bool hasWhipped;

    private PlayerMovement pm;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.state == PlayerMovement.State.whip)
        {
            hasWhipped = true;
            Debug.Log("WHIP");
        }
    }
}
