using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum TileTypes
{
    nullTile,
    beehive
}

public class HUDManager : MonoBehaviour
{
    private bool initialized = false;

    private Text pollenAmount = null;
    private Text nectarAmount = null;
    private Text honeyAmount = null;
    private Text waxAmount = null;
    private GameObject buildButton = null;
    private GameObject buildMenu = null;
    
    private TilemapManager tilemapManager = null;
    private ResourceManager resourceManager = null;

    // Start is called before the first frame update
    void Start()
    {
        InitializeReferences();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeReferences()
    {
        pollenAmount = gameObject.transform.Find("PollenAmount").GetComponent<Text>();
        nectarAmount = gameObject.transform.Find("NectarAmount").GetComponent<Text>();
        honeyAmount = gameObject.transform.Find("HoneyAmount").GetComponent<Text>();
        waxAmount = gameObject.transform.Find("WaxAmount").GetComponent<Text>();
        
        buildButton = gameObject.transform.Find("BuildButton").gameObject;
        buildMenu = gameObject.transform.Find("BuildMenu").gameObject;
        buildMenu.SetActive(false);
        
        tilemapManager = GameObject.Find("Grid").GetComponent<TilemapManager>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
    }

    public bool GetInitialized()
    {
        return initialized;
    }

    public void UpdateResourceAmount()
    {
        pollenAmount.text = "Pollen: " + resourceManager.GetPollen();
        nectarAmount.text = "Nectar: " + resourceManager.GetNectar();
        honeyAmount.text = "Honey: " + resourceManager.GetHoney();
        waxAmount.text = "Wax: " + resourceManager.GetWax();
    }

    public void BuildButton()
    {
        tilemapManager.inMenu = true;
        buildButton.SetActive(false);
        buildMenu.SetActive(true);
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
}
