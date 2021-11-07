using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoid : MonoBehaviour
{
    public bool isCarryingResources;

    public float speed = 3.0f;
    
    [SerializeField]
    private float minDist = 1.0f;
    [SerializeField]
    private float maxDist = 1.2f;
    
    [SerializeField]
    private float alignmentW = 0.9f;
    [SerializeField]
    private float separationW = 1.8f;
    [SerializeField]
    private float cohesionW = 0.8f;

    private List<GameObject> boids;
    
    private Rigidbody rb;

    public GameObject leader;
    public BoidManager manager;
    private TilemapManager tilemapManager;
    private ResourceManager resourceManager;

    // Start is called before the first frame update
    void Start()
    {
        leader = GameObject.Find("BoidLeader");
        manager = GameObject.Find("BoidManager").GetComponent<BoidManager>();
        resourceManager = FindObjectOfType<ResourceManager>();
        tilemapManager = FindObjectOfType<TilemapManager>();
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
                sepForce += new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0.0f);
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

    public void TryGatherResources()
    {
        if (tilemapManager.tileAvailablePollen[tilemapManager.grid.WorldToCell(leader.transform.position)]>0 && !isCarryingResources)
        {
            isCarryingResources = true;
            tilemapManager.tileAvailablePollen[tilemapManager.grid.WorldToCell(leader.transform.position)] -= 1;
        }
    }

    public void TryDeliverResources()
    {
        if (isCarryingResources)
        {
            isCarryingResources = false;
            resourceManager.AddPollen(1);
            resourceManager.AddHoney(1);
        }
    }
}
