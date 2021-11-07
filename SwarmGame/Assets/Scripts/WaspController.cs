using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaspController : MonoBehaviour
{
    private Grid grid;
    private TilemapManager tm;
    private HUDManager hudManager;
    private Vector3 targetPos;
    private Vector3Int nextCellPos;
    private bool hasMoveTarget = false;
    bool isMoving = false;
    public bool gameOver = false;

    private float moveTime = 3.0f;
    private float timeCounter = 0.0f;
    [SerializeField] private float speed = 2.0f;

    public Tile treeTile;
    public GameObject beeQueen;
    private BoidManager bm;
    
    // Start is called before the first frame update
    void Start()
    {
        beeQueen = GameObject.Find("BoidLeader");
        bm = GameObject.Find("BoidManager").GetComponent<BoidManager>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        hudManager = FindObjectOfType<HUDManager>();
        tm = grid.GetComponent<TilemapManager>();
        Vector3Int nearestNestPos = new Vector3Int(1000, 1000, 1000);
        
        for (int y = tm.objectsMap.origin.y; y < (tm.objectsMap.origin.y + tm.objectsMap.size.y); y++)
        {
            for (int x = tm.objectsMap.origin.x; x < (tm.objectsMap.origin.x + tm.objectsMap.size.x); x++)
            {
                TileBase tile = tm.objectsMap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    if (tile.name.Equals("Tree_Nest_01"))
                    {
                        if (Vector3.Distance(transform.position, new Vector3(x, y, 0)) < Vector3.Distance(transform.position, nearestNestPos))
                        {
                            nearestNestPos = new Vector3Int(x, y, 0);
                        }
                    }
                }
            }
        }

        targetPos = grid.CellToWorld(nearestNestPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (grid.WorldToCell(transform.position) == grid.WorldToCell(beeQueen.transform.position))
        {
            GameObject beeToDestroy = bm.getBoidList()[0];
            int removeIndex = 0;
            if (beeToDestroy.name == "BoidLeader" && bm.getBoidList().Count > 1)
            {
                beeToDestroy = bm.getBoidList()[1];
                removeIndex = 1;
            }
            Destroy(beeToDestroy);
            bm.getBoidList().RemoveAt(removeIndex);
            Destroy(gameObject);
        }
        timeCounter += Time.deltaTime;
        if (timeCounter >= moveTime && grid.WorldToCell(transform.position) != grid.WorldToCell(targetPos) && !isMoving)
        {
            Vector3Int waspPos = grid.WorldToCell(transform.position);
            Vector3Int target = grid.WorldToCell(targetPos);
            nextCellPos = new Vector3Int(0, 0, 0);

            int xDiff = Math.Abs(waspPos.x - target.x);
            int yDiff = Math.Abs(waspPos.y - target.y);

            if (xDiff > yDiff)
            {
                if (waspPos.x - target.x < 0)
                {
                    nextCellPos.x = waspPos.x + 1;
                    nextCellPos.y = waspPos.y;
                }
                else
                {
                    nextCellPos.x = waspPos.x - 1;
                    nextCellPos.y = waspPos.y;
                }
            }
            else
            {
                if (waspPos.y - target.y < 0)
                {
                    nextCellPos.y = waspPos.y + 1;
                    nextCellPos.x = waspPos.x;
                }
                else
                {
                    nextCellPos.y = waspPos.y - 1;
                    nextCellPos.x = waspPos.x;
                }
            }
            
            hasMoveTarget = true;

            timeCounter = 0.0f;
        }
        
        if (hasMoveTarget)
        {
            if (grid.CellToWorld(nextCellPos) == transform.position)
            {
                hasMoveTarget = false;
                timeCounter = 0.0f;
            }
            else
            {
                Vector3 direction = grid.CellToWorld(nextCellPos) - this.transform.position;
                if (direction.magnitude > 1)
                {
                    direction = Vector3.Normalize(direction);
                }

                this.transform.position += direction * Time.deltaTime * speed;
            }
        }

        if (tm.objectsMap.HasTile(grid.WorldToCell(transform.position)) && !isMoving)
        {
            if (tm.objectsMap.GetTile(grid.WorldToCell(transform.position)).name.Equals("Tree_Nest_01"))
            {
                tm.objectsMap.SetTile(grid.WorldToCell(transform.position), treeTile);
                tm.beeHiveCount--;
                hudManager.UpdateResourceAmount();
                if (tm.beeHiveCount == 0)
                {
                      gameOver = true;
                }

                if (gameOver)
                {
                    Debug.Log("GameOver is now true");
                }
                Destroy(gameObject);
            }
        }
    }
}
