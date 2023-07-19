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
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }
    }
}
