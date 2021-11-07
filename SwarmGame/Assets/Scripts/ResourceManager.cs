using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resources
{
    Pollen,
    Honey,
    Wax
}

public struct TileCosts
{
    public int Amount { get; }
    public Resources Material { get; }
    public TileCosts(int amount, Resources material)
    {
        Amount = amount;
        Material = material;
    }
}

public class ResourceManager : MonoBehaviour
{
    private int pollen;
    // private int nectar;
    private int honey;
    private int wax;
    
    private float honeyDecayTime = 5.0f;
    private float counter = 0.0f;

    private readonly int startingPollen = 100;
    // private readonly int startingNectar = 10;
    private readonly int startingHoney = 100;
    private readonly int startingWax = 100;

    private HUDManager hud = null;
    
    
    public Dictionary<TileTypes, TileCosts> tileCostMap = new Dictionary<TileTypes, TileCosts>();

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
                hud.ToggleDeathScreen();
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
        tileCostMap.Add(TileTypes.nullTile, new TileCosts(0, Resources.Pollen));
        tileCostMap.Add(TileTypes.flowers, new TileCosts(20, Resources.Pollen));
        tileCostMap.Add(TileTypes.tree, new TileCosts(40, Resources.Pollen));
        tileCostMap.Add(TileTypes.beehive, new TileCosts(25, Resources.Wax));
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
    public void AddHoney(int addedHoney)
    {
        honey += addedHoney;
        UpdateUI();
    }   
    public void AddWax(int addedWax)
    {
        wax += addedWax;
        UpdateUI();
    }

    public void ChangeHoney()
    {
        if (honey >= 30)
        {
            honey -= 30;
            wax += 5;

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
