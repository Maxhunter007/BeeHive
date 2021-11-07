using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaspManager : MonoBehaviour
{
    public GameObject wasp;
    
    private float waspSpawnTime = 15.0f;
    private int waspSpawnAmount = 1;
    private float waspSpawnTimer = 0.0f;
    private bool gameOver = false;
    
    private float diffOne = 60.0f;
    private float diffTwo = 120.0f;
    private float diffThree = 240.0f;
    private float difficultyCounter = 0.0f;
    private bool isMaxDiff = false;

    public Grid grid;
    private TilemapManager tm;
    private List<Vector3> possibleSpawnPositions = new List<Vector3>();
    private HUDManager hm;
    
    // Start is called before the first frame update
    void Start()
    {
        hm = GameObject.Find("HUD").GetComponent<HUDManager>();
        tm = grid.GetComponent<TilemapManager>();

        for (int y = tm.groundMap.origin.y; y < tm.groundMap.origin.y + tm.groundMap.size.y; y++)
        { 
            if (y == tm.groundMap.origin.y ||y == tm.groundMap.origin.y + tm.groundMap.size.y - 1) 
            {
                for (int x = tm.groundMap.origin.x; x < tm.groundMap.origin.x + tm.groundMap.size.x; x++)
                {
                    possibleSpawnPositions.Add(grid.CellToWorld(new Vector3Int(x, y, 0)));
                }
            }
            else
            {
                possibleSpawnPositions.Add(grid.CellToWorld(new Vector3Int(tm.groundMap.origin.x, y , 0)));
                possibleSpawnPositions.Add(grid.CellToWorld(new Vector3Int(tm.groundMap.origin.x + tm.groundMap.size.x - 1, y , 0)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMaxDiff)
        {
            difficultyCounter += Time.deltaTime;
        }
        
        waspSpawnTimer += Time.deltaTime;

        if (difficultyCounter >= diffOne && !isMaxDiff)
        {
            waspSpawnAmount = 2;
            if (difficultyCounter >= diffTwo)
            {
                waspSpawnAmount = 3;
                if (difficultyCounter >= diffThree)
                {
                    waspSpawnAmount = 5;
                    isMaxDiff = true;
                }
            }
        }

        if (waspSpawnTimer >= waspSpawnTime)
        {
            for (int i = 0; i < waspSpawnAmount; i++)
            {
                int randPos = Random.Range(0, possibleSpawnPositions.Count);
                GameObject w = Instantiate(wasp, possibleSpawnPositions[randPos], Quaternion.identity) as GameObject;
            }
            
            waspSpawnTimer = 0.0f;
        }
        
        gameOver = true;
        for (int y = tm.objectsMap.origin.y; y < (tm.objectsMap.origin.y + tm.objectsMap.size.y); y++)
        {
            for (int x = tm.objectsMap.origin.x; x < (tm.objectsMap.origin.x + tm.objectsMap.size.x); x++)
            {
                TileBase tile = tm.objectsMap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    if (tile.name.Equals("Tree_Nest_01"))
                    {
                        gameOver = false;
                    }
                }
            }
        }

        if (gameOver)
        {
            hm.ToggleDeathScreen();
        }
    }
}
