using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public enum TileTypes
{
    nullTile,
    flowers,
    tree,
    beehive
}

public class TilemapManager : MonoBehaviour
{
    public Grid grid = null;
    private Tilemap buyPreviewMap = null; 
    public Tilemap objectsMap = null;
    private Tilemap groundMap = null;
    private TileTypes hoverTileType = TileTypes.nullTile;
    private Tile hoverTile = null;
    public Tile flowers;
    public Tile tree;
    public Tile beeHiveTile;

    public bool buying = false;
    public bool inMenu = false;

    private List<Tile> buyableTiles = new List<Tile>();

    private Vector3Int previousMousePos = new Vector3Int();

    private HUDManager hudManager = null;
    private ResourceManager resourceManager = null;
    private Camera camera = null;

    private BoidManager boidManager;
    
    public Dictionary<Vector3Int, int> tileAvailableResources = new Dictionary<Vector3Int, int>();
    public List<Vector3Int> listOfFlowers = new List<Vector3Int>();

    private float regenerateResoureTime = 3.0f;
    private float regenerateResourceTimer = 0;
    

    // Start is called before the first frame update
    void Start() {
        InitializeReferences();
        InitializeDictionary();
        boidManager = GameObject.Find("BoidManager").GetComponent<BoidManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (buying)
        {
            BuyPreview();
        }
        
        ResourceRegeneration();
    }
    
    private void InitializeReferences()
    {
        grid = gameObject.GetComponent<Grid>();
        buyPreviewMap = gameObject.transform.Find("BuyPreviewMap").GetComponent<Tilemap>();
        objectsMap = gameObject.transform.Find("ObjectsMap").GetComponent<Tilemap>();
        groundMap = gameObject.transform.Find("GroundMap").GetComponent<Tilemap>();
        //buyableTiles.Add((Tile)AssetDatabase.LoadAssetAtPath("Assets/Tilemaps/Ground/testtile_hive.asset", typeof(Tile)));
        
        buyableTiles.Add(flowers);
        buyableTiles.Add(tree);
        buyableTiles.Add(beeHiveTile);
        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        camera = Camera.main;

    }

    private void InitializeDictionary()
    {
        Vector3Int currentpos;
        for (int x = objectsMap.cellBounds.xMin; x < objectsMap.cellBounds.xMax; x++)
        {
            for (int y = objectsMap.cellBounds.yMin; y < objectsMap.cellBounds.yMax; y++)
            {
                currentpos = new Vector3Int(x, y, 0);
                if (objectsMap.HasTile(currentpos) && objectsMap.GetTile(currentpos).name.Equals("Flowers_01"))
                {
                    tileAvailableResources.Add(currentpos, 10);
                    listOfFlowers.Add(currentpos);
                }
                else
                {
                    tileAvailableResources.Add(currentpos, 0);
                }
            }
        }
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
            if (objectsMap.HasTile(GetMousePosition()) && objectsMap.GetTile(GetMousePosition()).Equals(hoverTile))
            {
                abortBuying();
                return;
            }
            
            switch (resourceManager.tileCostMap[hoverTileType].Material)
            {
                
                case Resources.Pollen:
                    if (resourceManager.GetPollen()>=resourceManager.tileCostMap[hoverTileType].Amount)
                    {
                        if (hoverTileType == TileTypes.flowers)
                        {
                            if (objectsMap.HasTile(GetMousePosition()) && objectsMap.GetTile(GetMousePosition()).Equals(tree))
                            {
                                abortBuying();
                                break;
                            }
                            
                            listOfFlowers.Add(GetMousePosition());
                        }
                        
                        resourceManager.AddPollen(-resourceManager.tileCostMap[hoverTileType].Amount);
                        objectsMap.SetTile(mousePos, hoverTile);
                        
                        buying = false;
                        inMenu = false;
                        hudManager.EnableBuyButton();
                    }
                    else
                    {
                        abortBuying();
                    }
                    break;
                
                case Resources.Honey:
                    if (resourceManager.GetHoney()>=resourceManager.tileCostMap[hoverTileType].Amount)
                    {
                        resourceManager.AddHoney(-resourceManager.tileCostMap[hoverTileType].Amount);
                        objectsMap.SetTile(mousePos, hoverTile);
                                        
                        buying = false;
                        inMenu = false;
                        hudManager.EnableBuyButton();
                    }
                    else
                    {
                        abortBuying();
                    }
                    break;
                
                case Resources.Wax:
                    if (resourceManager.GetWax()>=resourceManager.tileCostMap[hoverTileType].Amount)
                    {
                        if (hoverTileType == TileTypes.beehive)
                        {
                            if (!objectsMap.HasTile(GetMousePosition()) || !objectsMap.GetTile(GetMousePosition()).Equals(tree))
                            {
                                abortBuying();
                                break;
                            }
                            
                            boidManager.createBoid(grid.CellToWorld(mousePos));
                        }
                        resourceManager.AddWax(-resourceManager.tileCostMap[hoverTileType].Amount);
                        objectsMap.SetTile(mousePos, hoverTile);

                        buying = false;
                        inMenu = false;
                        hudManager.EnableBuyButton();
                    }
                    else
                    {
                        abortBuying();
                    }
                    break;
                    
                default:
                    abortBuying();
                    break;
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
            case TileTypes.flowers:
                hoverTile = buyableTiles[0];
                break;
            case TileTypes.tree:
                hoverTile = buyableTiles[1];
                break; 
            case TileTypes.beehive:
                hoverTile = buyableTiles[2];
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

    private void ResourceRegeneration()
    {
        regenerateResourceTimer += Time.deltaTime;
        if (regenerateResourceTimer > regenerateResoureTime)
        {
            foreach (var flower in listOfFlowers)
            {
                if (tileAvailableResources[flower]<10)
                {
                    tileAvailableResources[flower] += 1;
                }
            }
            regenerateResourceTimer = 0f;
        }
    }
    
    
}
