using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private bool initialized = false;
    
    private Text pollenAmount = null;
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
        pollenAmount = gameObject.GetComponentInChildren<Text>();
    }

    public bool GetInitialized()
    {
        return initialized;
    }

    public void UpdatePollenAmount(int newPollen)
    {
        pollenAmount.text = "Pollen: " + newPollen;
    }
}
