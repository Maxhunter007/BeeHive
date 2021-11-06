using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private int pollen;
    // private int nectar;
    private int honey;
    private int wax;
    
    private float honeyDecayTime = 5.0f;
    private float counter = 0.0f;

    private readonly int startingPollen = 10;
    // private readonly int startingNectar = 10;
    private readonly int startingHoney = 10;
    private readonly int startingWax = 10;
    

    public readonly int hiveCost = 10;
    public readonly int treeCost = 10;
    public readonly int honeyCost = 10;
    public readonly int waxCost = 10;

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
        counter += Time.deltaTime;
        if (counter >= honeyDecayTime)
        {
            if (honey > 0)
            {
                honey--;
            }
            else
            {
                Application.Quit();
            }
            UpdateUI();
            counter = 0.0f;
        }
    }
    
    private void InitializeResources()
    {
        pollen = startingPollen;
        // nectar = startingNectar;
        honey = startingHoney;
        wax = startingWax;
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
    // public int GetNectar()
    // {
    //     return nectar;
    // }
    public int GetHoney()
    {
        return honey;
    }
    public int GetWax()
    {
        return wax;
    }

    public void AddPollen(int addedPollen)
    {
        pollen += addedPollen;
        UpdateUI();
    }

    public void ChangeHoney()
    {
        if (honey >= 6)
        {
            honey -= 6;
            wax++;
            UpdateUI();
        }
    }
    
    private void InitializeReferences()
    {
        hud = GameObject.Find("HUD").GetComponent<HUDManager>();
    }

    private void UpdateUI()
    {
        if (hud.GetInitialized())
        {
            hud.UpdateResourceAmount();
        }
        else
        {
            hud.InitializeReferences();
            hud.UpdateResourceAmount();
            // Debug.Log("Hud not initialized yet!");
        }
    }

    
}
