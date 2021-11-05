using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoid : MonoBehaviour
{
    public float speed = 5.0f;
    
    private float minDist = 3.0f;
    private float maxDist = 10.0f;

    private float alignmentW = 0.8f;
    private float separationW = 1.0f;
    private float cohesionW = 1.0f;

    private List<GameObject> boids;
    
    private Rigidbody rb;

    public GameObject leader;
    public BoidManager manager;

    // Start is called before the first frame update
    void Start()
    {
        leader = GameObject.Find("BoidLeader");
        manager = GameObject.Find("BoidManager").GetComponent<BoidManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        boids = manager.getBoidList();
        
        Vector3 force = new Vector3(0, 0, 0);

        force += alignmentW * Vector3.Normalize(alignment());
        force += separationW * Vector3.Normalize(separation(boids));
        force += cohesionW * Vector3.Normalize(cohesion(boids));

        force *= speed;

        rb.velocity = force;
    }

    Vector3 separation(List<GameObject> boids)
    {
        Vector3 sepForce = new Vector3(0, 0, 0);
        
        foreach (var boid in boids)
        {
            if (Vector3.Distance(boid.transform.position, this.transform.position) < minDist)
            {
                sepForce += this.transform.position - boid.transform.position;
            }
        }

        return sepForce;
    }

    Vector3 cohesion(List<GameObject> boids)
    {
        Vector3 cohForce = new Vector3(0, 0, 0);
        
        foreach (var boid in boids)
        {
            if (Vector3.Distance(boid.transform.position, this.transform.position) > maxDist)
            {
                cohForce += boid.transform.position - this.transform.position;
            }
        }

        return cohForce;
    }

    Vector3 alignment()
    {
        return leader.transform.position - this.transform.position;
    }

    public void setLeader(GameObject ld)
    {
        leader = ld;
    }
}
