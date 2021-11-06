using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class BoidLeader : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5.0f;
    public float maxVelocity = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.down);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right);
        }

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector3.Normalize(rb.velocity) * maxVelocity;
        }
    }
}
