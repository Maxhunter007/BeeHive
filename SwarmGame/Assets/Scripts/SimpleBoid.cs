using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoid : MonoBehaviour
{
    public float speed = 5.0f;
    
    private float minDist = 1.0f;
    private float maxDist = 3.0f;
    
    [SerializeField]
    private float alignmentW = 1.0f;
    [SerializeField]
    private float separationW = 0.8f;
    [SerializeField]
    private float cohesionW = 1.2f;

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
        
        this.transform.rotation = Quaternion.identity;
        rb.velocity = force;
        //this.transform.position += force;
    }

    Vector3 separation(List<GameObject> boids)
    {
        Vector3 sepForce = new Vector3(0, 0, 0);

        foreach (var boid in boids)
        {
            if (Vector3.Distance(boid.transform.position, this.transform.position) < minDist)
            {
                sepForce += Vector3.Normalize(this.transform.position - boid.transform.position);
            }

            if (Vector3.Distance(boid.transform.position, this.transform.position) == 0.0f)
            {
                sepForce += new Vector3(Random.Range(1.0f, 5.0f), Random.Range(1.0f, 5.0f), 0.0f);
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
