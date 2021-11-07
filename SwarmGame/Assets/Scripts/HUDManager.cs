using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private bool initialized = false;

    private Text pollenAmount = null;
    // private Text nectarAmount = null;
    private Text honeyAmount = null;
    private Text waxAmount = null;
    private Text BeeAmount = null;
    private GameObject buildButton = null;
    private GameObject buildMenu = null;
    
    private TilemapManager tilemapManager = null;
    private ResourceManager resourceManager = null;
    private BoidManager boidManager = null;

    // Start is called before the first frame update
    void Start()
    {
        InitializeReferences();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeReferences()
    {
        pollenAmount = GameObject.Find("PollenAmount").GetComponent<Text>();
        // nectarAmount = gameObject.transform.Find("NectarAmount").GetComponent<Text>();
        honeyAmount = GameObject.Find("HoneyAmount").GetComponent<Text>();
        waxAmount = GameObject.Find("WaxAmount").GetComponent<Text>();
        BeeAmount = GameObject.Find("BeeAmount").GetComponent<Text>();
        
        buildButton = gameObject.transform.Find("BuildButton").gameObject;
        boidManager = FindObjectOfType<BoidManager>();
        buildMenu = gameObject.transform.Find("BuildMenu").gameObject;
        buildMenu.SetActive(false);
        
        tilemapManager = GameObject.Find("Grid").GetComponent<TilemapManager>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        initialized = true;
        UpdateResourceAmount();
    }

    public bool GetInitialized()
    {
        return initialized;
    }

    public void UpdateResourceAmount()
    {
        pollenAmount.text = ": " + resourceManager.GetPollen();
        // nectarAmount.text = "Nectar: " + resourceManager.GetNectar();
        honeyAmount.text = ": " + resourceManager.GetHoney();
        waxAmount.text = ": " + resourceManager.GetWax();
        BeeAmount.text = ": " + (boidManager.getBoidList().Count-1) + "/" + tilemapManager.beeHiveCount;
    }

    public void ChangeHoneyButton()
    {
        resourceManager.ChangeHoney();
    }

    public void BuildButton()
    {
        tilemapManager.inMenu = true;
        buildButton.SetActive(false);
        buildMenu.SetActive(true);
    }

    public void BuyFlowersButton()
    {

        buildMenu.SetActive(false);
        tilemapManager.SelectTile(TileTypes.flowers);
        tilemapManager.buying = true;

    }
    public void BuyTreeButton()
    {

        buildMenu.SetActive(false);
        tilemapManager.SelectTile(TileTypes.tree);
        tilemapManager.buying = true;

    }
    public void BuyHiveButton()
    {

        buildMenu.SetActive(false);
        tilemapManager.SelectTile(TileTypes.beehive);
        tilemapManager.buying = true;

    }
    
    public void EnableBuyButton()
    {
        buildButton.SetActive(true);
    }

    public void rebuyBeeButton()
    {
        if (resourceManager.GetPollen()>25 && boidManager.getBoidList().Count-1 <  tilemapManager.beeHiveCount)
        {
            resourceManager.AddPollen(-25);
            boidManager.createBoid(FindObjectOfType<BoidLeader>().gameObject.transform.position);    
        }
        UpdateResourceAmount();
    }
}
