using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsandTransitions : MonoBehaviour
{
    public int levelToLoad;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() && other.GetComponent<PlayerMovement>().enteringDoor)
        {
            LevelLoader lL = FindAnyObjectByType<LevelLoader>();
            other.GetComponent<PlayerMovement>().EnterDoor();
            StartCoroutine(lL.LoadLevel(levelToLoad));
            
        }
    }
}
