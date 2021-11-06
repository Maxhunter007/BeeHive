﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public GameObject boid;
    private List<GameObject> boids = new List<GameObject>();
    public int boidCount = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            boids.Add(createBoid(Vector3.zero));
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
        return b;
    }

    public List<GameObject> getBoidList()
    {
        return boids;
    }
}
