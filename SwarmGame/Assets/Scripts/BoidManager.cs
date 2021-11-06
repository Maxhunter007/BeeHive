using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public GameObject boid;
    private List<GameObject> boids = new List<GameObject>();
    public int boidCount = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            createBoid(new Vector3(-1, -1, 0));
        }
        
        boids.Add(GameObject.Find("BoidLeader"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject createBoid(Vector3 position)
    {
        GameObject b = Instantiate(boid, position, Quaternion.identity) as GameObject;
        boids.Add(b);
        return b;
    }

    public List<GameObject> getBoidList()
    {
        return boids;
    }
}
