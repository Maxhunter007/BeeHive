using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private int pollen;
    private readonly int startingPollen = 100;

    public readonly int hiveCost = 10;

    private HUDManager hud = null;
    
    public Dictionary<TileTypes, int> tileCostMap = new Dictionary<TileTypes, int>();
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeReferences();
        InitializeResources();
        InitializeBuildCosts();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void InitializeResources()
    {
        pollen = startingPollen;
        UpdateUI();
    }
    private void InitializeBuildCosts()
    {
        tileCostMap.Add(TileTypes.nullTile, 0);
        tileCostMap.Add(TileTypes.beehive, 10);
    }

    public int GetPollen()
    {
        return pollen;
    }

    public void AddPollen(int addedPollen)
    {
        pollen += addedPollen;
        UpdateUI();
    }
    
    private void InitializeReferences()
    {
        hud = GameObject.Find("HUD").GetComponent<HUDManager>();
    }

    private void UpdateUI()
    {
        if (hud.GetInitialized())
        {
            hud.UpdatePollenAmount(pollen);
        }
        else
        {
            Debug.Log("Hud not initialized yet!");
        }
    }

    
}
