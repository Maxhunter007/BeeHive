using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class BoidLeader : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 targetPos = new Vector3();
    private bool hasTarget;
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
        if (Input.GetMouseButtonDown(0))
        {
            targetPos = Input.mousePosition;
            targetPos.z = 0.3f;
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);
            targetPos.z = 0;
            Debug.Log(targetPos);
            hasTarget = true;
            // Debug.Log("Mouse Click detected");
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
            // RaycastHit hit;  
            // if (Physics.Raycast(ray, out hit))
            // {  
            //     Debug.Log("Hit transform: " + hit.transform.position);
            //     //Select stage    
            //     if (hit.transform.name == "Plane")
            //     {
            //         targetPos = hit.transform;
            //         hasTarget = true;
            //     }  
            // }  
        }

        if (hasTarget)
        {
            if (this.transform.position != targetPos)
            {
                Vector3 direction = targetPos - this.transform.position;
                if (direction.magnitude > 1)
                {
                    direction = Vector3.Normalize(direction);
                }

                this.transform.position += direction * Time.deltaTime * speed;
            }
            else
            {
                hasTarget = false;
            }
        }
        
        // if (Input.GetKey(KeyCode.W))
        // {
        //     rb.AddForce(Vector3.up);
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     rb.AddForce(Vector3.left);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     rb.AddForce(Vector3.down);
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     rb.AddForce(Vector3.right);
        // }
        //
        // if (rb.velocity.magnitude > maxVelocity)
        // {
        //     rb.velocity = Vector3.Normalize(rb.velocity) * maxVelocity;
        // }
    }
}
