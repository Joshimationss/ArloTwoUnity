using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAi : MonoBehaviour
{
    public float enemyHealth;
    public float moveSpeed;

    public LayerMask whatisGround;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
