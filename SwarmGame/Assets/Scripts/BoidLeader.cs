using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class BoidLeader : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 targetPos = new Vector3();
    private bool hasTarget;
    private TilemapManager tm;
    
    public Grid grid;
    public float speed = 5.0f;
    public float maxVelocity = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tm = grid.GetComponent<TilemapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tm.inMenu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                targetPos = Input.mousePosition;
                targetPos.z = 0.3f;
                targetPos = Camera.main.ScreenToWorldPoint(targetPos);
                targetPos = grid.WorldToCell(targetPos);
                targetPos.z = 0;
                Debug.Log(targetPos);
                hasTarget = true;
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
        }
    }
}
