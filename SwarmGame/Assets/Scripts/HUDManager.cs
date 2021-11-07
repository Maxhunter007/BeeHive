using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private bool initialized = false;

    private Text pollenAmount = null;
    // private Text nectarAmount = null;
    private Text honeyAmount = null;
    private Text waxAmount = null;
    private GameObject buildButton = null;
    private GameObject buildMenu = null;
    public GameObject gameOverScreen;
    
    private TilemapManager tilemapManager = null;
    private ResourceManager resourceManager = null;
    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //InitializeReferences();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeReferences()
    {
        pollenAmount = gameObject.transform.Find("PollenAmount").GetComponent<Text>();
        // nectarAmount = gameObject.transform.Find("NectarAmount").GetComponent<Text>();
        honeyAmount = gameObject.transform.Find("HoneyAmount").GetComponent<Text>();
        waxAmount = gameObject.transform.Find("WaxAmount").GetComponent<Text>();
        
        buildButton = gameObject.transform.Find("BuildButton").gameObject;
        buildMenu = gameObject.transform.Find("BuildMenu").gameObject;
        buildMenu.SetActive(false);
        
        gameOverScreen.SetActive(false);
        
        tilemapManager = GameObject.Find("Grid").GetComponent<TilemapManager>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        initialized = true;
    }

    public bool GetInitialized()
    {
        return initialized;
    }

    public void UpdateResourceAmount()
    {
        pollenAmount.text = "Pollen: " + resourceManager.GetPollen();
        // nectarAmount.text = "Nectar: " + resourceManager.GetNectar();
        honeyAmount.text = "Honey: " + resourceManager.GetHoney();
        waxAmount.text = "Wax: " + resourceManager.GetWax();
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

    public void RetryButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ToggleDeathScreen()
    {
        if (!gameOver)
        { 
            gameOver = true;
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
        }
    }
}
