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
    private List<GameObject> boidList;

    public Grid grid;
    public float speed = 3.1f;
    public float maxVelocity = 10.0f;

    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tm = grid.GetComponent<TilemapManager>();
        boidList = FindObjectOfType<BoidManager>().getBoidList();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!tm.inMenu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)){
                    if (hit.transform.CompareTag("Ground"))
                    {
                        targetPos = Input.mousePosition;
                        targetPos.z = 0.3f;
                        targetPos = cam.ScreenToWorldPoint(targetPos);
                        targetPos = grid.CellToWorld(grid.WorldToCell(targetPos));
                        targetPos.z = 0;
                        Debug.Log(targetPos);
                        hasTarget = true;
                    }
                }
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
        else
        {
            targetPos = transform.position;
        }

        if (tm.objectsMap.HasTile(grid.WorldToCell(targetPos)))
        {
            if (tm.objectsMap.GetTile(grid.WorldToCell(targetPos)).name.Equals("Flowers_01"))
            {
                foreach (GameObject boid in boidList)
                {
                    if (boid.TryGetComponent(out SimpleBoid simpleBoid))
                    {
                        simpleBoid.TryGatherResources();
                    }
                }
            }
            else if (tm.objectsMap.GetTile(grid.WorldToCell(targetPos)).name.Equals("Flowers_01"))
            {
                
            }
        }
    }
}
