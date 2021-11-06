using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcenManager : MonoBehaviour
{
    private int pollen;
    private readonly int startingPollen = 100;

    [SerializeField] private HUDManager Hud = null;
    
    // Start is called before the first frame update
    void Start()
    {
        //InitializeReferences();
        InitializeResources();
        
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
        
    }

    private void UpdateUI()
    {
        if (Hud.GetInitialized())
        {
            Hud.UpdatePollenAmount(pollen);
        }
    }
}
