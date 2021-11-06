using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TilemapManager : MonoBehaviour
{
    private Grid grid = null;
    private Tilemap buyPreviewMap = null; 
    private Tilemap objectsMap = null;
    private TileTypes hoverTileType = TileTypes.nullTile;
    private Tile hoverTile = null;

    public bool buying = false;
    public bool inMenu = false;

    private List<Tile> buyableTiles = new List<Tile>();

    private Vector3Int previousMousePos = new Vector3Int();

    private HUDManager hudManager = null;
    private ResourceManager resourceManager = null;
    private Camera camera = null;

    private BoidManager boidManager;

    // Start is called before the first frame update
    void Start() {
        InitializeReferences();
        boidManager = GameObject.Find("BoidManager").GetComponent<BoidManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (buying)
        {
            BuyPreview();
        }
    }
    
    private void InitializeReferences()
    {
        grid = gameObject.GetComponent<Grid>();
        buyPreviewMap = gameObject.transform.Find("BuyPreviewMap").GetComponent<Tilemap>();
        objectsMap = gameObject.transform.Find("ObjectsMap").GetComponent<Tilemap>();
        buyableTiles.Add((Tile)AssetDatabase.LoadAssetAtPath("Assets/Tilemaps/Ground/testtile_hive.asset", typeof(Tile)));

        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        camera = Camera.main;

    }
    
    private Vector3Int GetMousePosition () {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    private void BuyPreview()
    {
        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos)) {
            buyPreviewMap.SetTile(previousMousePos, null);
            buyPreviewMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (resourceManager.GetPollen()>=resourceManager.tileCostMap[hoverTileType])
            {
                resourceManager.AddPollen(-resourceManager.hiveCost);
                objectsMap.SetTile(mousePos, hoverTile);
                boidManager.createBoid(grid.CellToWorld(mousePos));
                buying = false;
                inMenu = false;
                hudManager.EnableBuyButton();
            }
            else
            {
                abortBuying();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            abortBuying();
        }
    }

    public void SelectTile(TileTypes t)
    {
        hoverTileType = t;
        switch (t)
        {
            case TileTypes.nullTile:
                hoverTile = null;
                break;
            case TileTypes.beehive:
                hoverTile = buyableTiles[0];
                break;
            default:
                hoverTile = null;
                break;
        }
        
    }

    private void abortBuying()
    {
        buying = false;
        inMenu = false;
        SelectTile(TileTypes.nullTile);
        hudManager.EnableBuyButton();
        buyPreviewMap.SetTile(GetMousePosition(), null);
    }
    
    
}
