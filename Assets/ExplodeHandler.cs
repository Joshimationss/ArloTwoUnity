using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(KillSplode());
    }

    IEnumerator KillSplode()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
