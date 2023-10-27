using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject terrainMenuCanvas;
    [SerializeField] private GameObject terrainCanvas;

    // Start is called before the first frame update
    void Start() {

        ActivateMainMenu();
        GameManager.instance.OnMainMenu += ActivateMainMenu;
        GameManager.instance.OnTerrainMenu += ActivateTerrainMenu;
        GameManager.instance.OnTerrain += ActivateTerrain;
    }

    void ActivateMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        terrainMenuCanvas.SetActive(false);
        terrainCanvas.SetActive(false);
    }

    void ActivateTerrainMenu()
    {
        mainMenuCanvas.SetActive(false);
        terrainMenuCanvas.SetActive(true);
        terrainCanvas.SetActive(false);
    }

    void ActivateTerrain()
    {
        mainMenuCanvas.SetActive(false);
        terrainMenuCanvas.SetActive(false);
        terrainCanvas.SetActive(true);
    }

}
