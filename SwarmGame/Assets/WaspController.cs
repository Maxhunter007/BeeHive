using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaspController : MonoBehaviour
{
    private Grid grid;
    private TilemapManager tm;
    private Vector3 targetPos;
    private Vector3Int nextCellPos;
    private bool hasMoveTarget = false;
    bool isMoving = false;

    private float moveTime = 3.0f;
    private float timeCounter = 0.0f;
    [SerializeField] private float speed = 2.0f;

    public Tile treeTile;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
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
        
        Debug.Log("Nearest Nest was at: " + grid.CellToWorld(nearestNestPos));
        Debug.Log("Nearest Nest in Cell Coordinates: " + nearestNestPos);

        targetPos = grid.CellToWorld(nearestNestPos);
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= moveTime && grid.WorldToCell(transform.position) != grid.WorldToCell(targetPos) && !isMoving)
        {
            Vector3Int waspPos = grid.WorldToCell(transform.position);
            Vector3Int target = grid.WorldToCell(targetPos);
            Debug.Log("target is " + target);
            Debug.Log("position is " + waspPos);
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
                Destroy(gameObject);
            }
        }
    }
}
