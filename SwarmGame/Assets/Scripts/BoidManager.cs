﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public GameObject boid;
    private List<GameObject> boids = new List<GameObject>();
    public int boidCount = 3;
    public bool gameOver = false;
    public HUDManager hm;
    
    // Start is called before the first frame update
    void Start()
    {
        hm = GameObject.Find("HUD").GetComponent<HUDManager>();
        
        for (int i = 0; i < boidCount; i++)
        {
            createBoid(new Vector3(-1, -1, 0));
        }
        
        boids.Add(GameObject.Find("BoidLeader"));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            hm.ToggleDeathScreen();
        }
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
